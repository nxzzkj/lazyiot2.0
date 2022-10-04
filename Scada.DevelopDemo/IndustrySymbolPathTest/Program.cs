// See https://aka.ms/new-console-template for more information
using Scada.FlowGraphEngine;
using System.Drawing.Drawing2D;

Console.WriteLine("Hello, World!");
string str = "M218,104 l22,0 45,2 45,5 44,7 44,9 43,11 43,13 42,15 42,17 41,19 39,21 38,23 38,25 35,26 35,28 33,30 16,15";
Console.WriteLine("d="+ str);
 List <PathData> datas1=SVGAnalysisIndustrySymbol.SvgPathConvertGraphicsPathData(str);
string str2 = "M27,104 l191,0";
Console.WriteLine("d=" + str2);
List<PathData> datas3 = SVGAnalysisIndustrySymbol.SvgPathConvertGraphicsPathData(str2);

string str3 = "M217,27 l0,153";
Console.WriteLine("d=" + str3);
List<PathData> datas4 = SVGAnalysisIndustrySymbol.SvgPathConvertGraphicsPathData(str3);

string str4= "M863,370 l108,108";
Console.WriteLine("d=" + str4);
List<PathData> datas5 = SVGAnalysisIndustrySymbol.SvgPathConvertGraphicsPathData(str4);
string str5 = "M917,316 l-108,108";
Console.WriteLine("d=" + str5);
List<PathData> datas6 = SVGAnalysisIndustrySymbol.SvgPathConvertGraphicsPathData(str5);
