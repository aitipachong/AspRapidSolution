// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.Chart
// * 文件名称：		    OWCChart11.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-05-12
// * 程序功能描述：
// *        OWC(Microsoft Office Web Components)向Web添加图标功能类
// *        使用OWCChart11生成工作图表
// *        详情参考：http://www.cnblogs.com/peterzb/archive/2009/07/21/1527415.html
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using System;
using System.Data;
using System.Text;


namespace ItPachong.DoNet.Utilities.Chart
{
     /// <summary>
     /// 使用OWCChart11生成工作图表
     /// </summary>
    public class OWCChart11
    {
        #region 属性
        private string m_SavePath;
        private string m_Category;
        private string m_Value;
        private DataTable m_DataSource;
        private string m_SeriesName;
        private string m_Title;
        private string m_AxesXTitle;
        private string m_AxesYTitle;
        private int m_PicWidth;
        private int m_PicHeight;
        private int m_Type;
        private string m_FileName;

        /// <summary>
        /// 保存图片的路径和名称,物理路径
        /// </summary>
        public string SavePath
        {
            get { return m_SavePath; }
            set { m_SavePath = value; }
        }

        /// <summary>
        /// 直接获得类型
        /// </summary>
        public string OCategory
        {
            get { return m_Category; }
            set { m_Category = value; }
        }

        /// <summary>
        /// 直接获得值
        /// </summary>
        public string OValue
        {
            get { return m_Value; }
            set { m_Value = value; }
        }


        public DataTable DataSource
        {
            get { return m_DataSource; }
            set
            {
                m_DataSource = value;
                m_Category = GetColumnsStr(m_DataSource);
                m_Value = GetValueStr(m_DataSource);
            }
        }

        /// <summary>
        /// 简要说明
        /// </summary>
        public string SeriesName
        {
            get { return m_SeriesName; }
            set { m_SeriesName = value; }
        }

        /// <summary>
        /// 图表的总标题，说明图表的简单意思
        /// </summary>
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        /// <summary>
        /// 图表横坐标标题，说明横坐标的意义
        /// </summary>
        public string AxesXTitle
        {
            get { return m_AxesXTitle; }
            set { m_AxesXTitle = value; }
        }

        /// <summary>
        /// 图表纵坐标标题，说明纵坐标的意义
        /// </summary>
        public string AxesYTitle
        {
            get { return m_AxesYTitle; }
            set { m_AxesYTitle = value; }
        }

        /// <summary>
        ///  生成的图片宽度
        /// </summary>
        public int PicWidth
        {
            get { return m_PicWidth; }
            set { m_PicWidth = value; }
        }

        /// <summary>
        ///  生成的图片高度
        /// </summary>
        public int PicHeight
        {
            get { return m_PicHeight; }
            set { m_PicHeight = value; }
        }

        /// <summary>
        /// 类型
        ///chChartTypeColumnStacked100 =2 
        ///chChartTypeColumnStacked1003D = 49 
        ///chChartTypeColumnStacked3D = 48 
        ///chChartTypeCombo = -1 
        ///chChartTypeCombo3D = -2 
        ///chChartTypeDoughnut = 32 
        ///chChartTypeDoughnutExploded = 33 
        ///chChartTypeLine = 6 
        ///chChartTypeLine3D = 54 
        ///chChartTypeLineMarkers=  7 
        ///chChartTypeLineOverlapped3D=  55 
        ///chChartTypeLineStacked = 8 
        ///chChartTypeLineStacked100  =10 
        ///chChartTypeLineStacked1003D=  57 
        ///chChartTypeLineStacked100Markers = 11 
        ///chChartTypeLineStacked3D = 56 
        ///chChartTypeLineStackedMarkers = 9 
        ///chChartTypePie = 18 
        ///chChartTypePie3D =58 
        ///chChartTypePieExploded = 19 
        ///chChartTypePieExploded3D = 59 
        ///chChartTypePieStacked = 20 
        ///chChartTypePolarLine = 42 
        ///chChartTypePolarLineMarkers = 43 
        ///chChartTypePolarMarkers = 41 
        ///chChartTypePolarSmoothLine = 44 
        ///chChartTypePolarSmoothLineMarkers = 45 
        ///chChartTypeRadarLine=  34 
        ///chChartTypeRadarLineFilled = 36 
        ///chChartTypeRadarLineMarkers=  35 
        ///chChartTypeRadarSmoothLine = 37 
        ///chChartTypeRadarSmoothLineMarkers = 38 
        ///chChartTypeScatterLine = 25 
        ///chChartTypeScatterLineFilled = 26 
        ///chChartTypeScatterLineMarkers = 24 
        ///chChartTypeScatterMarkers = 21 
        ///chChartTypeScatterSmoothLine = 23 
        ///chChartTypeScatterSmoothLineMarkers = 22 
        ///chChartTypeSmoothLine = 12 
        ///chChartTypeSmoothLineMarkers = 13 
        ///chChartTypeSmoothLineStacked = 14 
        ///chChartTypeSmoothLineStacked100 = 16 
        ///chChartTypeSmoothLineStacked100Markers = 17 
        ///chChartTypeSmoothLineStackedMarkers = 15 
        ///chChartTypeStockHLC = 39 
        ///chChartTypeStockOHLC = 40 
        /// </summary>
        public int Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        /// <summary>
        /// 物理路径
        /// </summary>
        public string FileName
        {
            get { return m_FileName; }
            set
            {
                m_FileName = value;
                if(string.IsNullOrEmpty(m_FileName))
                {
                    m_FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".gif";
                }
            }
        }

        /// <summary>
        /// 获取DataTable列头名称
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetColumnsStr(DataTable dt)
        {
            StringBuilder strList = new StringBuilder();
            foreach(DataRow r in dt.Rows)
            {
                strList.Append(r[0].ToString() + "/t");
            }
            return strList.ToString();
        }

        /// <summary>
        /// 获取DataTable的值
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetValueStr(DataTable dt)
        {
            StringBuilder strList = new StringBuilder();
            foreach(DataRow r in dt.Rows)
            {
                strList.Append(r[1].ToString() + "/t");
            }
            return strList.ToString();
        }
        #endregion

        #region 枚举类型
        /// <summary>
        /// 枚举类型 对应于OWC的图表类型
        /// </summary>
        public enum ChartType : int
        {
            /// <summary>
            /// 面积图
            /// </summary>
            chChartTypeArea = 29,
            /// <summary>
            /// 面积图3D
            /// </summary>
            chChartTypeArea3D = 60,
            /// <summary>
            /// 面积图重复
            /// </summary>
            chChartTypeAreaOverlapped3D = 61,
            /// <summary>
            /// 堆积面积图
            /// </summary>
            chChartTypeAreaStacked = 30,
            /// <summary>
            /// 堆积面积图百分比图
            /// </summary>
            chChartTypeAreaStacked100 = 31,
            /// <summary>
            /// 堆积面积图百分比图3D
            /// </summary>
            chChartTypeAreaStacked1003D = 63,
            /// <summary>
            /// 堆积面积图3D
            /// </summary>
            chChartTypeAreaStacked3D = 62,
            /// <summary>
            /// 横道图3D
            /// </summary>
            chChartTypeBar3D = 50,
            /// <summary>
            /// 横道图串风格
            /// </summary>
            chChartTypeBarClustered = 3,
            /// <summary>
            /// 横道图串风格3D
            /// </summary>
            chChartTypeBarClustered3D = 51,
            /// <summary>
            /// 堆横道图
            /// </summary>
            chChartTypeBarStacked = 4,
            /// <summary>
            /// 堆横道图百分比图
            /// </summary>
            chChartTypeBarStacked100 = 5,
            /// <summary>
            /// 堆横道图百分比图3D
            /// </summary>
            chChartTypeBarStacked1003D = 53,
            /// <summary>
            /// 堆横道图3D
            /// </summary>
            chChartTypeBarStacked3D = 52,
            /// <summary>
            /// 气泡图
            /// </summary>
            chChartTypeBubble = 27,
            /// <summary>
            /// 线形气泡图
            /// </summary>
            chChartTypeBubbleLine = 28,
            /// <summary>
            /// 柱形图
            /// </summary>
            chChartTypeColumn3D = 46,
            /// <summary>
            /// 3D柱形图
            /// </summary>
            chChartTypeColumnClustered = 0,
            /// <summary>
            /// 3D串柱形图
            /// </summary>
            chChartTypeColumnClustered3D = 47,
            /// <summary>
            /// 重叠柱形图
            /// </summary>
            chChartTypeColumnStacked = 1,
            /// <summary>
            /// 100%重叠柱形图
            /// </summary>
            chChartTypeColumnStacked100 = 2,
            /// <summary>
            /// 100%3D重叠柱形图
            /// </summary>
            chChartTypeColumnStacked1003D = 49,
            /// <summary>
            /// 3D柱形图
            /// </summary>
            chChartTypeColumnStacked3D = 48,
            /// <summary>
            /// 组合图
            /// </summary>
            chChartTypeCombo = -1,
            /// <summary>
            /// 3D组合图
            /// </summary>
            chChartTypeCombo3D = -2,
            /// <summary>
            /// 环形图
            /// </summary>
            chChartTypeDoughnut = 32,
            /// <summary>
            /// 破式环形图
            /// </summary>
            chChartTypeDoughnutExploded = 33,
            /// <summary>
            /// 折线图
            /// </summary>
            chChartTypeLine = 6,
            /// <summary>
            /// 3D折线图
            /// </summary>
            chChartTypeLine3D = 54,
            /// <summary>
            /// 制造折线图
            /// </summary>
            chChartTypeLineMarkers = 7,
            /// <summary>
            /// 重复折线图
            /// </summary>
            chChartTypeLineOverlapped3D = 55,
            /// <summary>
            /// 重叠折线图
            /// </summary>
            chChartTypeLineStacked = 8,
            /// <summary>
            /// 100%重叠折线图
            /// </summary>
            chChartTypeLineStacked100 = 10,
            /// <summary>
            /// 100%3D重叠折线图
            /// </summary>
            chChartTypeLineStacked1003D = 57,
            /// <summary>
            /// 制造100%重叠折线图
            /// </summary>
            chChartTypeLineStacked100Markers = 11,
            /// <summary>
            /// 3D重叠折线图
            /// </summary>
            chChartTypeLineStacked3D = 56,
            /// <summary>
            /// 制造重叠折线图
            /// </summary>
            chChartTypeLineStackedMarkers = 9,
            /// <summary>
            /// 饼图
            /// </summary>
            chChartTypePie = 18,
            /// <summary>
            /// 3D饼图
            /// </summary>
            chChartTypePie3D = 58,
            /// <summary>
            /// 破式饼图
            /// </summary>
            chChartTypePieExploded = 19,
            /// <summary>
            /// 3D破式饼图
            /// </summary>
            chChartTypePieExploded3D = 59,
            /// <summary>
            /// 重叠饼图
            /// </summary>
            chChartTypePieStacked = 20,
            /// <summary>
            /// 极坐标图
            /// </summary>
            chChartTypePolarLine = 42,
            /// <summary>
            /// 制造线形极坐标图
            /// </summary>
            chChartTypePolarLineMarkers = 43,
            /// <summary>
            /// 制造极坐标图
            /// </summary>
            chChartTypePolarMarkers = 41,
            /// <summary>
            /// 平滑线形极坐标图
            /// </summary>
            chChartTypePolarSmoothLine = 44,
            /// <summary>
            /// 制造平滑线形极坐标图
            /// </summary>
            chChartTypePolarSmoothLineMarkers = 45,
            /// <summary>
            /// 雷达图
            /// </summary>
            chChartTypeRadarLine = 34,
            /// <summary>
            /// 填充雷达图
            /// </summary>
            chChartTypeRadarLineFilled = 36,
            /// <summary>
            /// 制造雷达图
            /// </summary>
            chChartTypeRadarLineMarkers = 35,
            /// <summary>
            /// 平滑雷达图
            /// </summary>
            chChartTypeRadarSmoothLine = 37,
            /// <summary>
            /// 制造平滑雷达图
            /// </summary>
            chChartTypeRadarSmoothLineMarkers = 38,
            /// <summary>
            /// 线形散点图
            /// </summary>
            chChartTypeScatterLine = 25,
            /// <summary>
            /// 填充线形散点图
            /// </summary>
            chChartTypeScatterLineFilled = 26,
            /// <summary>
            /// 制造线形散点图
            /// </summary>
            chChartTypeScatterLineMarkers = 24,
            /// <summary>
            /// 制造散点图
            /// </summary>
            chChartTypeScatterMarkers = 21,
            /// <summary>
            /// 平滑散点图
            /// </summary>
            chChartTypeScatterSmoothLine = 23,
            /// <summary>
            /// 制造平滑散点图
            /// </summary>
            chChartTypeScatterSmoothLineMarkers = 22,
            /// <summary>
            /// 平滑线图
            /// </summary>
            chChartTypeSmoothLine = 12,
            /// <summary>
            /// 制造平滑线图
            /// </summary>
            chChartTypeSmoothLineMarkers = 13,
            /// <summary>
            /// 重叠平滑线图
            /// </summary>
            chChartTypeSmoothLineStacked = 14,
            /// <summary>
            /// 100%重叠平滑线图
            /// </summary>
            chChartTypeSmoothLineStacked100 = 16,
            /// <summary>
            /// 制造100%重叠平滑线图
            /// </summary>
            chChartTypeSmoothLineStacked100Markers = 17,
            /// <summary>
            /// 制造重叠平滑线图
            /// </summary>
            chChartTypeSmoothLineStackedMarkers = 15,
            /// <summary>
            /// 股价图
            /// </summary>
            chChartTypeStockHLC = 39,
            /// <summary>
            /// 股价图O型
            /// </summary>
            chChartTypeStockOHLC = 40
        }
        #endregion

        #region 构造函数

        public OWCChart11()
        {
        }

        public OWCChart11(string savePath, string seriesName, string title, int chartType)
        {
            m_SavePath = savePath;
            m_SeriesName = seriesName;
            m_Title = title;
            m_Type = chartType;
        }

        public OWCChart11(string SavePath, string SeriesName, string Title, int ChartType, string AxesXTitle, string AxesYTitle)
        {
            m_SavePath = SavePath;
            m_SeriesName = SeriesName;
            m_Title = Title;
            m_AxesXTitle = AxesXTitle;
            m_AxesYTitle = AxesYTitle;
            m_Type = ChartType;
        }

        public OWCChart11(string SavePath, string SeriesName, string Title, int ChartType, string AxesXTitle, string AxesYTitle, int PicWidth, int PicHeight)
        {
            m_SavePath = SavePath;
            m_SeriesName = SeriesName;
            m_Title = Title;
            m_AxesXTitle = AxesXTitle;
            m_AxesYTitle = AxesYTitle;
            m_PicWidth = PicWidth;
            m_PicHeight = PicHeight;
            m_Type = ChartType;
        }
        #endregion

        /// <summary>
        /// 创建图表
        /// </summary>
        /// <returns></returns>
        public bool Create()
        {
            //声明对象
            Microsoft.Office.Interop.Owc11.ChartSpace ThisChart = new Microsoft.Office.Interop.Owc11.ChartSpace();// .ChartSpaceClass();
            Microsoft.Office.Interop.Owc11.ChChart ThisChChart = ThisChart.Charts.Add(0);
            Microsoft.Office.Interop.Owc11.ChSeries ThisChSeries = ThisChChart.SeriesCollection.Add(0);

            //显示图例
            ThisChChart.HasLegend = true;

            //显示标题选项
            ThisChChart.HasTitle = true;
            ThisChChart.Title.Font.Name = "黑体";
            ThisChChart.Title.Font.Size = 14;
            ThisChChart.Title.Caption = m_Title;

            //x,y轴说明
            ThisChChart.Axes[0].HasTitle = true;
            ThisChChart.Axes[0].Title.Font.Name = "黑体";
            ThisChChart.Axes[0].Title.Font.Size = 12;
            ThisChChart.Axes[0].Title.Caption = m_AxesXTitle;
            ThisChChart.Axes[1].HasTitle = true;
            ThisChChart.Axes[1].Title.Font.Name = "黑体";
            ThisChChart.Axes[1].Title.Font.Size = 12;
            ThisChChart.Axes[1].Title.Caption = m_AxesYTitle;

            //图表类型
            ThisChChart.Type = (Microsoft.Office.Interop.Owc11.ChartChartTypeEnum)m_Type;
            
            //旋转
            ThisChChart.Rotation = 360;
            ThisChChart.Inclination = 10;

            //背景色
            ThisChChart.PlotArea.Interior.Color = "red";
            //底座色
            ThisChChart.PlotArea.Floor.Interior.Color = "green";

            //ThisChChart.Overlap = 50;

            //给定series的名字
            ThisChSeries.SetData(Microsoft.Office.Interop.Owc11.ChartDimensionsEnum.chDimSeriesNames, 
                Microsoft.Office.Interop.Owc11.ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), m_SeriesName);
            //给定分类
            ThisChSeries.SetData(Microsoft.Office.Interop.Owc11.ChartDimensionsEnum.chDimCategories,
                Microsoft.Office.Interop.Owc11.ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), m_Category);
            //给定值
            ThisChSeries.SetData(Microsoft.Office.Interop.Owc11.ChartDimensionsEnum.chDimValues,
                Microsoft.Office.Interop.Owc11.ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), m_Value);

            Microsoft.Office.Interop.Owc11.ChDataLabels dl = ThisChChart.SeriesCollection[0].DataLabelsCollection.Add();
            dl.HasValue = true;
            //dl.HasPercentage = true;

            //导出图像文件
            try
            {
                if(string.IsNullOrEmpty(m_FileName))
                {
                    m_FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".gif";
                }

                ThisChart.ExportPicture(m_SavePath + "//" + m_FileName, "gif", m_PicWidth, m_PicHeight);
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}