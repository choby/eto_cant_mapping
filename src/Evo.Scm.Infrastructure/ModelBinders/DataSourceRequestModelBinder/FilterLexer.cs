using System.Text;
using Kendo.Mvc.Infrastructure.Implementation;

namespace Evo.Scm.ModelBinders.DataSourceRequestModelBinder;

 public class FilterLexer
  {
    private const char Separator = '~';
    private static readonly string[] ComparisonOperators = new string[6]
    {
      "eq",
      "neq",
      "lt",
      "lte",
      "gt",
      "gte"
    };
    private static readonly string[] LogicalOperators = new string[3]
    {
      "and",
      "or",
      "not"
    };
    private static readonly string[] Booleans = new string[2]
    {
      "true",
      "false"
    };
    private static readonly string[] Functions = new string[11]
    {
      "contains",
      "endswith",
      "startswith",
      "notsubstringof",
      "doesnotcontain",
      "isnull",
      "isnotnull",
      "isempty",
      "isnotempty",
      "isnullorempty",
      "isnotnullorempty"
    };
    private int currentCharacterIndex;
    private readonly string input;

    public FilterLexer(string input)
    {
      input = input ?? string.Empty;
      this.input = input.Trim(new char[1]{ '~' });
    }

    public IList<FilterToken> Tokenize()
    {
      List<FilterToken> filterTokenList = new List<FilterToken>();
      while (this.currentCharacterIndex < this.input.Length)
      {
        string result;
        if (this.TryParseIdentifier(out result))
          filterTokenList.Add(this.Identifier(result));
        else if (this.TryParseNumber(out result))
          filterTokenList.Add(FilterLexer.Number(result));
        else if (this.TryParseString(out result))
          filterTokenList.Add(FilterLexer.String(result));
        else if (this.TryParseCharacter(out result, '('))
          filterTokenList.Add(FilterLexer.LeftParenthesis(result));
        else if (this.TryParseCharacter(out result, ')'))
        {
          filterTokenList.Add(FilterLexer.RightParenthesis(result));
        }
        else
        {
          if (!this.TryParseCharacter(out result, ','))
            throw new FilterParserException("Expected token");
          filterTokenList.Add(FilterLexer.Comma(result));
        }
      }
      return (IList<FilterToken>) filterTokenList;
    }

    private static bool IsComparisonOperator(string value) => Array.IndexOf<string>(FilterLexer.ComparisonOperators, value) > -1;

    private static bool IsLogicalOperator(string value) => Array.IndexOf<string>(FilterLexer.LogicalOperators, value) > -1;

    private static bool IsBoolean(string value) => Array.IndexOf<string>(FilterLexer.Booleans, value) > -1;

    private static bool IsFunction(string value) => Array.IndexOf<string>(FilterLexer.Functions, value) > -1;

    private static FilterToken Comma(string result) => new FilterToken()
    {
      TokenType = FilterTokenType.Comma,
      Value = result
    };

    private static FilterToken Boolean(string result) => new FilterToken()
    {
      TokenType = FilterTokenType.Boolean,
      Value = result
    };

    private static FilterToken RightParenthesis(string result) => new FilterToken()
    {
      TokenType = FilterTokenType.RightParenthesis,
      Value = result
    };

    private static FilterToken LeftParenthesis(string result) => new FilterToken()
    {
      TokenType = FilterTokenType.LeftParenthesis,
      Value = result
    };

    private static FilterToken String(string result) => new FilterToken()
    {
      TokenType = FilterTokenType.String,
      Value = result.Trim(' ').Replace("\t","").Replace("\r","").Replace("\n","")
    };

    private static FilterToken Number(string result) => new FilterToken()
    {
      TokenType = FilterTokenType.Number,
      Value = result
    };

    private FilterToken Date(string result)
    {
      this.TryParseString(out result);
      return new FilterToken()
      {
        TokenType = FilterTokenType.DateTime,
        Value = result
      };
    }

    private FilterToken Null(string result) => new FilterToken()
    {
      TokenType = FilterTokenType.Null,
      Value = (string) null
    };

    private static FilterToken ComparisonOperator(string result) => new FilterToken()
    {
      TokenType = FilterTokenType.ComparisonOperator,
      Value = result
    };

    private static FilterToken LogicalOperator(string result)
    {
      switch (result)
      {
        case "or":
          return new FilterToken()
          {
            TokenType = FilterTokenType.Or,
            Value = result
          };
        case "and":
          return new FilterToken()
          {
            TokenType = FilterTokenType.And,
            Value = result
          };
        default:
          return new FilterToken()
          {
            TokenType = FilterTokenType.Not,
            Value = result
          };
      }
    }

    private static FilterToken Function(string result) => new FilterToken()
    {
      TokenType = FilterTokenType.Function,
      Value = result
    };

    private static FilterToken Property(string result) => new FilterToken()
    {
      TokenType = FilterTokenType.Property,
      Value = result
    };

    private FilterToken Identifier(string result)
    {
      if (result == "datetime")
        return this.Date(result);
      if (FilterLexer.IsComparisonOperator(result))
        return FilterLexer.ComparisonOperator(result);
      if (FilterLexer.IsLogicalOperator(result))
        return FilterLexer.LogicalOperator(result);
      if (FilterLexer.IsBoolean(result))
        return FilterLexer.Boolean(result);
      if (FilterLexer.IsFunction(result))
        return FilterLexer.Function(result);
      return result == "null" || result == "undefined" ? this.Null(result) : FilterLexer.Property(result);
    }

    private void SkipSeparators()
    {
      char ch = this.Peek();
      while (ch == '~')
        ch = this.Next();
    }

    private bool TryParseCharacter(out string character, char whatCharacter)
    {
      this.SkipSeparators();
      char ch = this.Peek();
      if ((int) ch != (int) whatCharacter)
      {
        character = (string) null;
        return false;
      }
      int num = (int) this.Next();
      character = ch.ToString();
      return true;
    }

    private bool TryParseString(out string @string)
    {
      this.SkipSeparators();
      if (this.Peek() != '\'')
      {
        @string = (string) null;
        return false;
      }
      int num1 = (int) this.Next();
      StringBuilder result = new StringBuilder();
      @string = this.Read((Func<char, bool>) (character =>
      {
        if (character == char.MaxValue)
          throw new FilterParserException("Unterminated string");
        if (character != '\'' || this.Peek(1) != '\'')
          return character != '\'';
        int num2 = (int) this.Next();
        return true;
      }), result);
      int num3 = (int) this.Next();
      return true;
    }

    private bool TryParseNumber(out string number)
    {
      this.SkipSeparators();
      char c = this.Peek();
      StringBuilder result = new StringBuilder();
      int decimalSymbols = 0;
      if (c == '-' || c == '+')
      {
        result.Append(c);
        c = this.Next();
      }
      if (c == '.')
      {
        decimalSymbols++;
        result.Append(c);
        c = this.Next();
      }
      if (!char.IsDigit(c))
      {
        number = (string) null;
        return false;
      }
      number = this.Read((Func<char, bool>) (character =>
      {
        if (character != '.')
          return char.IsDigit(character);
        if (decimalSymbols >= 1)
          throw new FilterParserException("A number cannot have more than one decimal symbol");
        ++decimalSymbols;
        return true;
      }), result);
      return true;
    }

    private bool TryParseIdentifier(out string identifier)
    {
      this.SkipSeparators();
      char character1 = this.Peek();
      StringBuilder result = new StringBuilder();
      if (!FilterLexer.IsIdentifierStart(character1))
      {
        identifier = (string) null;
        return false;
      }
      result.Append(character1);
      int num = (int) this.Next();
      identifier = this.Read((Func<char, bool>) (character => FilterLexer.IsIdentifierPart(character) || character == '.'), result);
      return true;
    }

    private static bool IsIdentifierPart(char character) => char.IsLetter(character) || char.IsDigit(character) || character == '_' || character == '$';

    private static bool IsIdentifierStart(char character) => char.IsLetter(character) || character == '_' || character == '$' || character == '@';

    private string Read(Func<char, bool> predicate, StringBuilder result)
    {
      for (char ch = this.Peek(); predicate(ch); ch = this.Next())
        result.Append(ch);
      return result.ToString();
    }

    private char Peek() => this.Peek(0);

    private char Peek(int chars) => this.currentCharacterIndex + chars < this.input.Length ? this.input[this.currentCharacterIndex + chars] : char.MaxValue;

    private char Next()
    {
      ++this.currentCharacterIndex;
      return this.Peek();
    }
  }