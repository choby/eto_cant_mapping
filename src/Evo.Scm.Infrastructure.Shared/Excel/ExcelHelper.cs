using System.ComponentModel.DataAnnotations;
using System.Globalization;
using NPOI.HSSF.UserModel;
using System.Data;
using System.Reflection;
using Evo.Scm.Net;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Masuit.Tools.Database;

namespace Evo.Scm.Excel
{
    public class ExcelHelper
    {

        private const string DynamicMember = "dynamic";
        
        /// <summary>
        /// 初始化
        /// </summary>
        public static XSSFWorkbook InitializeWorkbook()
        {
            XSSFWorkbook _XSSFWorkbook = new XSSFWorkbook();

            //  DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            //  dsi.Company = "";
            //((HSSFWorkbook) _XSSFWorkbook).DocumentSummaryInformation = dsi;

            //  SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            //  si.Subject = "";
            //  _XSSFWorkbook.SummaryInformation = si;

            return _XSSFWorkbook;
        }

        /// <summary>
        /// 自定义列名转对应实体字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static DataTable ConvertColumns<T>(DataTable dataTable)
        {
            foreach (var propertyInfo in typeof(T).GetProperties().Where(x => x.GetCustomAttributes<ExcelColumnAttribute>().Any()))
            {
                //实体特性
                var customAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(ExcelColumnAttribute));
                if (customAttribute == null)
                    continue;
                var title = ((ExcelColumnAttribute)customAttribute).Title;
                var name = propertyInfo.Name;
                //数据源列头
                var dataColumn = dataTable.Columns[title];
                if (dataColumn == null)
                    continue;
                dataColumn.ColumnName = name;
            }
            return dataTable;
        }

        /// <summary>
        /// 实体类属性转对应特性列头
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static DataTable ConvertAttributes<T>(DataTable dataTable)
        {
            foreach (var propertyInfo in typeof(T).GetProperties().Where(x => x.GetCustomAttributes<ExcelColumnAttribute>().Any()))
            {
                //实体特性
                var customAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(ExcelColumnAttribute));
                if (customAttribute == null)
                    continue;
                var title = ((ExcelColumnAttribute)customAttribute).Title;
                var name = propertyInfo.Name;
                //数据源列头
                var dataColumn = dataTable.Columns[name];
                if (dataColumn == null)
                    continue;
                dataColumn.ColumnName = title;
            }
            return dataTable;
        }
        /// <summary>
        /// 导入Excel转List
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<T> ReadToDataTable<T>(Stream stream,string fileName) where T : class, new()
        {
            var dtResult = ReadToDataTable(stream, fileName);
            ConvertColumns<T>(dtResult);
            return dtResult.ToList<T>();
        }

        public static DataTable ReadToDataTable(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return ReadToDataTable(stream, filePath);
            }
        }
        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">二进制流</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DataTable ReadToDataTable(Stream stream,string fileName)
        {
            DataTable dtResult = new DataTable();
            string fileExt = Path.GetExtension(fileName).ToLower();
            IWorkbook workbook;
            try
            {
                if (fileExt == ".xlsx")
                    workbook = new XSSFWorkbook(stream);
                else
                    workbook = new HSSFWorkbook(stream);
            }
            catch
            {
                string errorExt = fileExt == ".xlsx" ? ".xls" : fileExt;
                throw new Exception($"{fileExt}后缀与文件不匹配，请使用{errorExt}后缀");
            }
            ISheet sheet = workbook.GetSheetAt(0);
            //获取sheet的首行
            IRow headerRow = sheet.GetRow(0);
            for (int i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue.Replace("*", "").Trim());
                dtResult.Columns.Add(column);
            }
            for (int i = (sheet.FirstRowNum + 1); i < (sheet.LastRowNum + 1); i++)
            {

                IRow row = sheet.GetRow(i);
                if (row == null) throw new Exception("读取excel出错了没有获取到数据");
                DataRow dataRow = dtResult.NewRow();
                bool allColumnIsNull = true;
                string strTamp = string.Empty;
                for (int j = row.FirstCellNum; j < headerRow.LastCellNum; j++)
                {
                    var cell = row.GetCell(j);
                    if (cell != null)
                    {
                        switch (cell.CellType)
                        {
                            case CellType.Blank: //空数据类型处理
                                dataRow[j] = "";
                                break;
                            case CellType.String: //字符串类型
                                strTamp = cell.StringCellValue.Trim();
                                dataRow[j] = strTamp;
                                allColumnIsNull = string.IsNullOrEmpty(strTamp);
                                break;
                            case CellType.Numeric: //数字类型                                   
                                if (HSSFDateUtil.IsCellDateFormatted(cell))
                                {
                                    dataRow[j] = cell.DateCellValue;
                                }
                                else
                                {
                                    dataRow[j] = cell.NumericCellValue;
                                }
                                allColumnIsNull = false;
                                break;
                            case CellType.Formula:
                                HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(sheet.Workbook);
                                dataRow[j] = e.Evaluate(cell).StringValue;
                                allColumnIsNull = false;
                                break;
                            default:
                                dataRow[j] = "";
                                break;
                        }
                    }
                }
                if (!allColumnIsNull)
                    dtResult.Rows.Add(dataRow);
            }

            return dtResult;
        }

        public static DataTable ReadToDataTable(Stream stream,
                                         int sheetIndex,
                                         int firstRowNum = 0,
                                         int firstCellNum = 0,
                                         int lastCellNum = 0)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("序号");

            ISheet sheet = new XSSFWorkbook(stream).GetSheetAt(sheetIndex);
            firstRowNum = firstRowNum == 0 ? sheet.FirstRowNum : firstRowNum;
            int lastRowRum = sheet.LastRowNum;
            //获取sheet的首行
            IRow headerRow = sheet.GetRow(firstRowNum);
            firstCellNum = firstCellNum == 0 ? headerRow.FirstCellNum : firstCellNum;
            lastCellNum = lastCellNum == 0 ? headerRow.LastCellNum : lastCellNum;
            for (int i = firstCellNum; i <= lastCellNum; i++)
            {
                dtResult.Columns.Add(headerRow.GetCell(i).StringCellValue);
            }

            for (int i = (firstRowNum + 1); i <= lastRowRum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null)
                    throw new Exception(string.Format("读取第[{0}]行数据出错.", i));

                DataRow dataRow = dtResult.NewRow();
                bool allColumnIsNull = true;
                string strTamp = string.Empty;
                dataRow[0] = row.RowNum.ToString(CultureInfo.InvariantCulture);
                for (int j = firstCellNum; j <= lastCellNum; j++)
                {
                   
                    var cell = row.GetCell(j);
                    if (cell != null)
                    {
                        switch (cell.CellType)
                        {
                            case CellType.Blank: //空数据类型处理
                                dataRow[j+1] = "";
                                break;
                            case CellType.String: //字符串类型
                                strTamp = cell.StringCellValue.Trim();
                                dataRow[j + 1] = strTamp;
                                allColumnIsNull = string.IsNullOrEmpty(strTamp);
                                break;
                            case CellType.Numeric: //数字类型                                   
                                if (HSSFDateUtil.IsCellDateFormatted(cell))
                                {
                                    dataRow[j + 1] = cell.DateCellValue;
                                }
                                else
                                {
                                    dataRow[j + 1] = cell.NumericCellValue;
                                }
                                allColumnIsNull = false;
                                break;
                            case CellType.Formula:
                                HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(sheet.Workbook);
                                dataRow[j + 1] = e.Evaluate(cell).StringValue;
                                allColumnIsNull = false;
                                break;
                            default:
                                dataRow[j + 1] = "";
                                break;
                        }
                    }
                }
                if (!allColumnIsNull)
                    dtResult.Rows.Add(dataRow);
            }
            return dtResult;
        }

        public static (MemoryStream, string) ConvertToExcel<T>(IEnumerable<T> data)
        {
            XSSFWorkbook workbook = InitializeWorkbook();

            var modelType = typeof(T);
            var sheetName = modelType.GetCustomAttributes<ExcelAttribute>().Select(x => x.Name).FirstOrDefault();
            var columns = new List<ExcelColumn>();
            int index = 0;
            
            foreach (var propertyInfo in modelType.GetProperties().Where(x => x.GetCustomAttributes<ExcelColumnAttribute>().Any() || x.GetCustomAttributes<DynamicColumnAttribute>().Any()))
            {
                var excelColumn = propertyInfo.GetCustomAttributes<ExcelColumnAttribute>().FirstOrDefault();
                var excelImage = propertyInfo.GetCustomAttributes<ExcelImageAttribute>().FirstOrDefault();
                // var dynamicColumn = propertyInfo.GetCustomAttributes<DynamicColumnAttribute>().FirstOrDefault();
                if (propertyInfo.PropertyType.IsArray)
                {
                    var row = data.ElementAt(0);
                    var type = row.GetType();
                    if (propertyInfo.GetValue(row) is string[] array)
                    {
                        for (int i = 0; i < array.Length; i++)
                        {
                            columns.Add(new ExcelColumn()
                            {
                                Member = DynamicMember,
                                Title = array[i],
                                MemberType = typeof(decimal),
                                Width = excelColumn?.Width ?? 0
                            });
                        }
                    }
                }
                else
                {
                    columns.Add(new ExcelColumn()
                    {
                        Member = propertyInfo.Name,
                        Title = excelColumn?.Title,
                        MemberType = propertyInfo.PropertyType,
                        Width = excelColumn?.Width ?? 0,
                        ExcelImage = excelImage is not null
                            ? new ExcelImage
                            {
                                Width = excelImage.Width,
                                Height = excelImage.Height
                            }
                            : null
                    });
                }

                index++;
            }

            if (sheetName != null)
            {
                ListExcel(sheetName, data, columns, workbook);
            }

            return (WriteToStream(workbook), sheetName);
        }
        
        public static MemoryStream WriteToStream(XSSFWorkbook workbook)
        {
            
            var file = new MemoryStream();
            workbook.Write(file,true);
            file.Seek(0, SeekOrigin.Begin);
            // file.Flush();
            return file;
        }

        /// <summary>
        /// DataTable写入Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="data"></param>
        /// <param name="imageFieldName"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <returns></returns>
        public static MemoryStream ConvertToExcel(string sheetName, DataTable data,string? imageFieldName = null, int imageWidth = 100, int imageHeight = 100)
        {
            //初始化Excel信息
            XSSFWorkbook workbook = InitializeWorkbook();

            //填充数据
            DTExcel(sheetName, data, null, workbook, imageFieldName, imageWidth, imageHeight);

            return WriteToStream(workbook);
        }

        #region 数据填充部分

        /// <summary>
        /// 将DataTable数据写入到Excel
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="dt"></param>
        /// <param name="lstTitle"></param>
        /// <param name="workbook"></param>
        /// <param name="imageFieldName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        static void DTExcel(string sheetName, DataTable dt, IList<string> lstTitle, XSSFWorkbook workbook,string? imageFieldName, int? width , int? height)
        {
            XSSFWorkbook _XSSFWorkbook = workbook;

            ISheet sheet1 = _XSSFWorkbook.CreateSheet(sheetName);
            int y = dt.Columns.Count;
            int x = dt.Rows.Count;

            //给定的标题为空,赋值datatable默认的列名
            if (lstTitle == null)
            {
                lstTitle = new List<string>();
                for (int ycount = 0; ycount < y; ycount++)
                { lstTitle.Add(dt.Columns[ycount].Caption); }
            }

            IRow hsTitleRow = sheet1.CreateRow(0);
            //标题赋值
            for (int yt = 0; yt < lstTitle.Count; yt++)
            { hsTitleRow.CreateCell(yt).SetCellValue(lstTitle[yt]); }

            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //填充数据项
            for (int xcount = 0; xcount < x; xcount++)
            {
                IRow hsBodyRow = sheet1.CreateRow(xcount + 1);

                for (int ycBody = 0; ycBody < y; ycBody++)
                {
                    var newCell = hsBodyRow.CreateCell(ycBody);//.SetCellValue(dt.DefaultView[xcount][ycBody].ToString());
                    if (!string.IsNullOrWhiteSpace(imageFieldName) && dt.Columns[ycBody].ColumnName == imageFieldName)
                    {
                        AddPieChart((XSSFCell)newCell, dt.DefaultView[xcount][ycBody]?.ToString());
                        if(width.HasValue)
                            sheet1.SetColumnWidth(ycBody, width.Value * 256 / 6);
                        if (height.HasValue)
                            hsBodyRow.HeightInPoints = height.Value;
                    }
                    else
                    {
                        switch (dt.Columns[ycBody].DataType.ToString())
                        {
                            case "System.String": //字符串类型
                                newCell.SetCellValue(dt.DefaultView[xcount][ycBody]?.ToString());
                                break;
                            case "System.DateTime": //日期类型
                                DateTime dateV;
                                DateTime.TryParse(dt.DefaultView[xcount][ycBody]?.ToString(), out dateV);
                                if ((new DateTime()).Date == dateV)
                                {
                                    newCell.SetCellValue("");
                                }
                                else
                                {
                                    newCell.SetCellValue(dateV);
                                    newCell.CellStyle = dateStyle;
                                }

                                break;
                            case "System.Boolean": //布尔型
                                bool boolV = false;
                                bool.TryParse(dt.DefaultView[xcount][ycBody]?.ToString(), out boolV);
                                newCell.SetCellValue(boolV);
                                break;
                            case "System.Int16": //整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(dt.DefaultView[xcount][ycBody]?.ToString(), out intV);
                                newCell.SetCellValue(intV);
                                break;
                            case "System.Decimal": //浮点型
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(dt.DefaultView[xcount][ycBody]?.ToString(), out doubV);
                                newCell.SetCellValue(doubV);
                                break;
                            case "System.DBNull": //空值处理
                                newCell.SetCellValue("");
                                break;
                            default:
                                newCell.SetCellValue("");
                                break;
                        }
                    }
                }
            }

        }

        static void ListExcel<T>(string sheetName, IList<T> lst, IList<string> lstTitle, XSSFWorkbook workbook)
        {
            XSSFWorkbook _XSSFWorkbook = workbook;

            ISheet sheet1 = _XSSFWorkbook.CreateSheet(sheetName);

            T _t = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] propertys = _t.GetType().GetProperties();

            //给定的标题为空,赋值T默认的列名
            if (lstTitle == null)
            {
                lstTitle = propertys.Select(t => t.Name).ToList();
            }

            IRow hsTitleRow = sheet1.CreateRow(0);
            //标题赋值
            for (int yt = 0; yt < lstTitle.Count; yt++)
            {
                hsTitleRow.CreateCell(yt).SetCellValue(lstTitle[yt]);
            }

            //填充数据项
            for (int xcount = 0; xcount < lst.Count; xcount++)
            {
                IRow hsBodyRow = sheet1.CreateRow(xcount + 1);

                for (int ycBody = 0; ycBody < lstTitle.Count; ycBody++)
                {
                    PropertyInfo pi = propertys.First(p => p.Name == lstTitle[ycBody]);
                    if (!lstTitle.Any(t => t == pi.Name))
                        continue;
                    object obj = pi.GetValue(lst[xcount], null);
                    if (obj != null)
                    {
                        hsBodyRow.CreateCell(ycBody).SetCellValue(obj.ToString());
                    }
                    else
                    {
                        hsBodyRow.CreateCell(ycBody).SetCellValue("");

                    }
                }
            }

        }

        private static void ListExcel<T>(string sheetName, IEnumerable<T> data, IList<ExcelColumn> columns, XSSFWorkbook workbook)
        {
           
            if (columns == null || columns.Count < 1)
            {
                throw new ArgumentNullException("columns", "必须配置导出的列");
            }

            var sheet = workbook.CreateSheet(sheetName);
            IList<int> imageColumns = new List<int>();
            #region 创建标题行

            IRow hsTitleRow = sheet.CreateRow(0);
            //标题赋值
            for (int i = 0; i < columns.Count; i++)
            {
                var cell = hsTitleRow.CreateCell(i);
                string strTitle = columns[i].Title;
                int intWidth = columns[i].Width > 0
                    ? Math.Min(columns[i].Width, 255)
                    : 10;
                sheet.SetColumnWidth(i, intWidth * 256);
                cell.SetCellValue(strTitle);

                if (columns[i].ExcelImage is not null)
                {
                    sheet.SetColumnWidth(i, columns[i].ExcelImage.Width * 256);
                    imageColumns.Add(i);
                }
            }

            #endregion

            //填充数据项
            // PropertyInfo[] arrProperty = Activator.CreateInstance(typeof(T)).GetType().GetProperties();
            try
            {
                ICellStyle shortDateStyle = workbook.CreateCellStyle();
                IDataFormat format = workbook.CreateDataFormat();
                shortDateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

                ICellStyle longDateStyle = workbook.CreateCellStyle();
                longDateStyle.DataFormat = format.GetFormat("yyyy-mm-dd HH:mm:ss");

                ICellStyle percentStyle = workbook.CreateCellStyle();
                percentStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
                int dynamicValueIndex = 0;
                for (int xcount = 0; xcount < data.Count(); xcount++)
                {

                    #region 写一行数据
                    IRow hsBodyRow = sheet.CreateRow(xcount + 1);

                    for (int ycBody = 0; ycBody < columns.Count; ycBody++)
                    {
                        var row = data.ElementAt(xcount);

                        if (columns[ycBody].Member == DynamicMember)
                        {
                            var info = row.GetType().GetProperties().FirstOrDefault(x => x.PropertyType.IsGenericType && x.GetCustomAttributes<DynamicValueAttribute>().Any());
                            if (info is not null)
                            {
                                var dynamicValues = info.GetValue(row);
                                var item = dynamicValues.GetType().GetProperty("Item").GetValue(dynamicValues,new object[]{dynamicValueIndex});
                                var decimalValue = item.GetType().GetProperty("Quantity").GetValue(item);
                                if (decimalValue is not null)
                                {
                                    Int32.TryParse(decimalValue.ToString(), out int cellValue);
                                    var createCell = hsBodyRow.CreateCell(ycBody);
                                    createCell.SetCellValue(cellValue);
                                }
                            }
                            dynamicValueIndex++;
                        }
                        else
                        {
                            object obj = row.GetType().GetProperty(columns[ycBody].Member).GetValue(row); //columns[ycBody].GetColumnValue(data[xcount]);
                            if (columns[ycBody].ExcelImage is not null) //如果是图片列
                            {
                                if (obj != null && obj.ToString().Trim() != "")
                                {
                                    // if (!File.Exists(obj.ToString().Trim()))
                                    //     continue;
                                    hsBodyRow.Height = (short)(columns[ycBody].ExcelImage.Height * 20);
                                    //HSSFCell cell=hsBodyRow.CreateCell(ycBody) as HSSFCell;
                                    //AddPieChart(cell, obj.ToString(), 100, 100);
                                    AddPieChart(sheet, workbook, obj.ToString(), xcount + 1, ycBody,
                                        columns[ycBody].ExcelImage.Width, columns[ycBody].ExcelImage.Height);
                                }
                            }
                            else
                            {
                                var createCell = hsBodyRow.CreateCell(ycBody);

                                if (columns[ycBody].MemberType.FullName == typeof(Int32?).FullName ||
                                    columns[ycBody].MemberType.Name == typeof(Int32).Name)
                                {
                                    int intValue;
                                    Int32.TryParse(obj?.ToString(), out intValue);
                                    createCell.SetCellValue(intValue);
                                }
                                else if (columns[ycBody].MemberType.FullName == typeof(double?).FullName ||
                                         columns[ycBody].MemberType.Name == typeof(double).Name ||
                                         columns[ycBody].MemberType.FullName == typeof(decimal?).FullName ||
                                         columns[ycBody].MemberType.Name == typeof(decimal).Name)
                                {
                                    double doubleValue;
                                    if (obj?.ToString().Contains("%") == true)
                                    {
                                        double.TryParse(obj.ToString().TrimEnd('%'), out doubleValue);
                                        createCell.SetCellValue(doubleValue / 100);
                                        createCell.CellStyle = percentStyle;
                                    }
                                    else if (obj?.ToString().Contains("¥") == true)
                                    {
                                        double.TryParse(obj.ToString().TrimStart('¥'), out doubleValue);
                                        createCell.SetCellValue(doubleValue);
                                    }
                                    else
                                    {
                                        double.TryParse(obj?.ToString(), out doubleValue);
                                        createCell.SetCellValue(doubleValue);
                                    }
                                }
                                else if (columns[ycBody].MemberType.Name == "System.DBNull")
                                {
                                    createCell.SetCellValue("");
                                }
                                else if (columns[ycBody].MemberType.FullName == typeof(DateTime?).FullName ||
                                         columns[ycBody].MemberType.Name == typeof(DateTime).Name)
                                {
                                    DateTime dateV;
                                    DateTime.TryParse(obj?.ToString(), out dateV);
                                    if ((new DateTime()).Date == dateV)
                                    {
                                        createCell.SetCellValue("");
                                    }
                                    else
                                    {
                                        createCell.SetCellValue(dateV);

                                        #region 设置数据显示格式

                                        var attributes = Activator.CreateInstance(typeof(T))
                                            .GetType()
                                            .GetProperty(columns[ycBody].Member)
                                            .GetCustomAttributes(typeof(DisplayFormatAttribute));

                                        var displayFormatAttribute =
                                            attributes.FirstOrDefault() as DisplayFormatAttribute;
                                        if (displayFormatAttribute?.DataFormatString == "{0:yyyy-MM-dd HH:mm:ss}")
                                        {
                                            createCell.CellStyle = longDateStyle;
                                        }
                                        else
                                        {
                                            createCell.CellStyle = shortDateStyle;
                                        }

                                        #endregion
                                    }
                                }
                                else
                                {
                                    createCell.SetCellValue(obj?.ToString());
                                }
                            }
                        }
                    }

                    #endregion
                }
            }
            catch //(Exception ex)
            {
            }
        }

        #endregion

        #region 向sheet插入图片
        ///
        /// 向sheet插入图片
        ///
        private static void AddPieChart(ISheet sheet, XSSFWorkbook workbook, string picpath, int row, int col, int imageWidth = 0, int imageHeight = 0)
        {
            try
            {
                // string FileName = picpath;
                // if (!File.Exists(FileName))
                //     return;

                //分配内存流
                var stream = HttpClientHelper.GetFile(picpath).Result; //new MemoryStream(4096);
                //裁剪图片
                // var resizeSetting = new ResizeSettings { Width = 100, Height = 100, Scale = ScaleMode.Both };
                //stream.Seek(0, SeekOrigin.Begin);
                //byte[] bytes = HttpClientHelper.GetFile(picpath); //System.IO.File.ReadAllBytes(picpath);

                //裁剪图片
                // new ImageJob(new MemoryStream(bytes), stream, resizeSetting).Build();
                //stream.Seek(0, SeekOrigin.Begin);

                if (!string.IsNullOrEmpty(picpath))
                {
                    
                    int pictureIdx = workbook.AddPicture(stream.GetAllBytes(), PictureType.JPEG);
                    var drawing = sheet.CreateDrawingPatriarch();
                    // XSSFDrawing patriarch = (XSSFDrawing)sheet.CreateDrawingPatriarch();
                    XSSFClientAnchor anchor = new XSSFClientAnchor(0, 0, 0, 0, col, row, col + 1, row + 1);
                    var picture = drawing.CreatePicture(anchor, pictureIdx);
                    picture.Resize();
                    //##处理照片位置，【图片左上角为（col, row）第row+1行col+1列，右下角为（ col +1, row +1）第 col +1+1行row +1+1列，宽为100，高为50
                    // XSSFPicture pict = (XSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                    //缩略图已经是最终符合单元格的尺寸，把整个缩略图完全显示出来，不需要再做缩放
                    //pict.Resize();
                }

            }
            catch //(Exception ex)
            {
            }
        }
        #endregion

        #region 向sheet插入图片
        /// <summary>
        /// 向sheet插入图片
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="picpath"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        public static void AddPieChart(XSSFCell cell, string picpath, int imageWidth = 0, int imageHeight = 0)
        {
            try
            {
                string FileName = picpath;
                

                if (!string.IsNullOrEmpty(FileName))
                {
                    //分配内存流
                    var stream = HttpClientHelper.GetFile(picpath).Result; //new MemoryStream(4096);
                    //裁剪图片
                    // var resizeSetting = new ResizeSettings { Width = imageWidth, Height = imageHeight, Scale = ScaleMode.Both };
                    //if(!(width > 0 && height > 0))
                    //    resizeSetting.Mode = FitMode.Carve;
                    //裁剪图片
                    // new ImageJob(new MemoryStream(bytes), stream, resizeSetting).Build();
                    //stream.Seek(0, SeekOrigin.Begin);

                    int pictureIdx = cell.Sheet.Workbook.AddPicture(stream.GetAllBytes(), PictureType.JPEG);
                    var drawing = cell.Sheet.CreateDrawingPatriarch();
                    var anchor = new XSSFClientAnchor(0, 0, 0, 0, cell.ColumnIndex, cell.RowIndex, cell.ColumnIndex + 1, cell.RowIndex + 1);
                    //##处理照片位置，【图片左上角为（col, row）第row+1行col+1列，右下角为（ col +1, row +1）第 col +1+1行row +1+1列，宽为100，高为50

                    var pict = drawing.CreatePicture(anchor, pictureIdx);
                    //缩略图已经是最终符合单元格的尺寸，把整个缩略图完全显示出来，不需要再做缩放
                    //pict.Resize();

                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

      
    }

}
