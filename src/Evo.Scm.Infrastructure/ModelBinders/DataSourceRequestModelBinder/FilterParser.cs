using System.Globalization;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure.Implementation;

namespace Evo.Scm.ModelBinders.DataSourceRequestModelBinder;

public class FilterParser
  {
    private readonly IList<FilterToken> tokens;
    private int currentTokenIndex;

    public FilterParser(string input) => this.tokens = new FilterLexer(input).Tokenize();

    public IFilterNode Parse() => this.tokens.Count > 0 ? this.Expression() : (IFilterNode) null;

    private IFilterNode Expression() => this.OrExpression();

    private IFilterNode OrExpression()
    {
      IFilterNode firstArgument = this.AndExpression();
      if (this.Is(FilterTokenType.Or))
        return this.ParseOrExpression(firstArgument);
      if (!this.Is(FilterTokenType.And))
        return firstArgument;
      this.Expect(FilterTokenType.And);
      return (IFilterNode) new AndNode()
      {
        First = firstArgument,
        Second = this.OrExpression()
      };
    }

    private IFilterNode AndExpression()
    {
      IFilterNode firstArgument = this.ComparisonExpression();
      return this.Is(FilterTokenType.And) ? this.ParseAndExpression(firstArgument) : firstArgument;
    }

    private IFilterNode ComparisonExpression()
    {
      IFilterNode firstArgument = this.PrimaryExpression();
      return this.Is(FilterTokenType.ComparisonOperator) || this.Is(FilterTokenType.Function) ? this.ParseComparisonExpression(firstArgument) : firstArgument;
    }

    private IFilterNode PrimaryExpression()
    {
      if (this.Is(FilterTokenType.LeftParenthesis))
        return this.ParseNestedExpression();
      if (this.Is(FilterTokenType.Function))
        return this.ParseFunctionExpression();
      if (this.Is(FilterTokenType.Boolean))
        return this.ParseBoolean();
      if (this.Is(FilterTokenType.DateTime))
        return this.ParseDateTimeExpression();
      if (this.Is(FilterTokenType.Property))
        return this.ParsePropertyExpression();
      if (this.Is(FilterTokenType.Number))
        return this.ParseNumberExpression();
      if (this.Is(FilterTokenType.String))
        return this.ParseStringExpression();
      if (this.Is(FilterTokenType.Null))
        return this.ParseNullExpression();
      throw new FilterParserException("Expected primaryExpression");
    }

    private IFilterNode ParseOrExpression(IFilterNode firstArgument)
    {
      this.Expect(FilterTokenType.Or);
      IFilterNode filterNode = this.OrExpression();
      return (IFilterNode) new OrNode()
      {
        First = firstArgument,
        Second = filterNode
      };
    }

    private IFilterNode ParseComparisonExpression(IFilterNode firstArgument)
    {
      if (this.Is(FilterTokenType.ComparisonOperator))
      {
        FilterToken token = this.Expect(FilterTokenType.ComparisonOperator);
        IFilterNode filterNode = this.PrimaryExpression();
        return (IFilterNode) new ComparisonNode()
        {
          First = firstArgument,
          FilterOperator = token.ToFilterOperator(),
          Second = filterNode
        };
      }
      FilterToken token1 = this.Expect(FilterTokenType.Function);
      FunctionNode comparisonExpression = new FunctionNode();
      comparisonExpression.FilterOperator = token1.ToFilterOperator();
      comparisonExpression.Arguments.Add(firstArgument);
      comparisonExpression.Arguments.Add(this.PrimaryExpression());
      return (IFilterNode) comparisonExpression;
    }

    private IFilterNode ParseAndExpression(IFilterNode firstArgument)
    {
      this.Expect(FilterTokenType.And);
      IFilterNode filterNode = this.ComparisonExpression();
      return (IFilterNode) new AndNode()
      {
        First = firstArgument,
        Second = filterNode
      };
    }

    private IFilterNode ParseNullExpression()
    {
      FilterToken filterToken = this.Expect(FilterTokenType.Null);
      return (IFilterNode) new NullNode()
      {
        Value = (object) filterToken.Value
      };
    }

    private IFilterNode ParseStringExpression()
    {
      FilterToken filterToken = this.Expect(FilterTokenType.String);
      return (IFilterNode) new StringNode()
      {
        Value = (object) filterToken.Value
      };
    }

    private IFilterNode ParseBoolean()
    {
      FilterToken filterToken = this.Expect(FilterTokenType.Boolean);
      return (IFilterNode) new BooleanNode()
      {
        Value = (object) Convert.ToBoolean(filterToken.Value)
      };
    }

    private IFilterNode ParseNumberExpression()
    {
      FilterToken filterToken = this.Expect(FilterTokenType.Number);
      return (IFilterNode) new NumberNode()
      {
        Value = (object) Convert.ToDouble(filterToken.Value, (IFormatProvider) CultureInfo.InvariantCulture)
      };
    }

    private IFilterNode ParsePropertyExpression()
    {
      FilterToken filterToken = this.Expect(FilterTokenType.Property);
      return (IFilterNode) new PropertyNode()
      {
        Name = filterToken.Value
      };
    }

    private IFilterNode ParseDateTimeExpression()
    {
      FilterToken filterToken = this.Expect(FilterTokenType.DateTime);
      return (IFilterNode) new DateTimeNode()
      {
        Value = (object) DateTime.ParseExact(filterToken.Value, "yyyy-MM-ddTHH-mm-ss", (IFormatProvider) null)
      };
    }

    private IFilterNode ParseNestedExpression()
    {
      this.Expect(FilterTokenType.LeftParenthesis);
      IFilterNode nestedExpression = this.Expression();
      this.Expect(FilterTokenType.RightParenthesis);
      return nestedExpression;
    }

    private IFilterNode ParseFunctionExpression()
    {
      FilterToken token = this.Expect(FilterTokenType.Function);
      FunctionNode functionExpression = new FunctionNode()
      {
        FilterOperator = token.ToFilterOperator()
      };
      this.Expect(FilterTokenType.LeftParenthesis);
      functionExpression.Arguments.Add(this.Expression());
      while (this.Is(FilterTokenType.Comma))
      {
        this.Expect(FilterTokenType.Comma);
        functionExpression.Arguments.Add(this.Expression());
      }
      this.Expect(FilterTokenType.RightParenthesis);
      return (IFilterNode) functionExpression;
    }

    private FilterToken Expect(FilterTokenType tokenType)
    {
      if (!this.Is(tokenType))
        throw new FilterParserException("Expected " + tokenType.ToString());
      FilterToken filterToken = this.Peek();
      ++this.currentTokenIndex;
      return filterToken;
    }

    private bool Is(FilterTokenType tokenType)
    {
      FilterToken filterToken = this.Peek();
      return filterToken != null && filterToken.TokenType == tokenType;
    }

    private FilterToken Peek() => this.currentTokenIndex < this.tokens.Count ? this.tokens[this.currentTokenIndex] : (FilterToken) null;
  }