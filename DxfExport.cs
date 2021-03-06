﻿using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Linq;
using WW.Cad.Base;
using WW.Cad.Drawing;
using WW.Cad.Drawing.GDI;
using WW.Cad.IO;
using WW.Cad.Model;
using WW.Cad.Model.Entities;
using WW.Cad.Model.Tables;
using WW.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;

namespace Cadl

{
    public class CADEntity
    {
        
        public string type;
        public int color;
        public string transform;
        public ulong parentHandle;
    }
    public class CADLine : CADEntity
    {
        public string startPoint;
        public string endPoint;
        public int lineWeight;

    }

    public class CADLwPolyLine : CADEntity
    {
        public string[] vertices;
        public string closed;
    }

    public class CADSpline : CADEntity
    {
        public string[] fitPoints;
    }

    public class CADCircle : CADEntity
    {
        public string center;
        public double radius;
    }

    public class CADArc : CADEntity
    {
        public string center;
        public double radius;
        public double startAngle;
        public double endAngle;
    }

    public class CADEllipse : CADEntity
    {
        public string center;
        public string majorAxisEndPoint;
        public string minorAxisEndPoint;
        public double startParameter;
        public double endParameter;
    }

    public class CADMText : CADEntity
    {
        public string simplifiedText;
        public string fontStyle;
        public double size;

        public string attachmentPoint;
        public double boxHeight;
        public double boxWidth;
    }

    public class CADText : CADEntity
    {
        public string simplifiedText;
        public string fontStyle;
        public double size;

        public string alignMentPoint1;
        public double rotationAngle; 
    }

    public class CADInsert : CADEntity
    {
        public string insertPoint;
        public double rotationAngle;
        public string insertName;
        public ulong nowHandle;
        public string insertScale;
    }


    public class DxfExport
    {
        static List<CADEntity> cadEntities = new List<CADEntity>();

        static FileStream fs = new FileStream("D:\\C项目\\CadLCmd\\CadLCmd\\txt\\test3.txt", FileMode.Create);

        static StreamWriter sw = new StreamWriter(fs, Encoding.Default);

        static DxfLine dxfLine = null;

        static DxfLwPolyline dxfLwPolyline = null;

        static DxfXLine dxfXLine = null;

        static DxfSpline dxfSpline = null;

        static DxfCircle dxfCircle = null;

        static DxfArc dxfArc = null;

        static DxfEllipse dxfEllipse = null;

        static DxfMText dxfMText = null;

        static DxfText dxfText = null;

        static DxfInsert dxfInsert = null;

        static DxfAttributeDefinition dxfAttributeDefinition = null;

        static DxfEntityCollection dxfEntityCollection = null;

        static DxfBlockBegin dxfBlockBegin = null;

        static DxfBlock dxfBlock = null;

        static DxfMText[] KTCodes = new DxfMText[0]; //机型逗号拆分
        static List<DxfMText> ktls = KTCodes.ToList();

        static DxfText[] KTest = new DxfText[0]; //机型逗号拆分
        static List<DxfText> ktest = KTest.ToList();

        static String[] DxfType = new String[0];
        static List<String> dxfType = DxfType.ToList();

        static DxfInsert[] KInsert = new DxfInsert[0]; //机型逗号拆分
        static List<DxfInsert> kins = KInsert.ToList();

        static DxfEntityCollection[] KEC = new DxfEntityCollection[0]; //机型逗号拆分
        static List<DxfEntityCollection> kco = KEC.ToList();

        static DxfCircle[] KCIRCLE = new DxfCircle[0]; //机型逗号拆分
        static List<DxfCircle> kcircle = KCIRCLE.ToList();



        static void FindEntities(DxfEntityCollection Entities)
        {
            
            foreach (var entityGroups in Entities)
            {

                if (typeof(DxfLine) != entityGroups.GetType() && typeof(DxfLwPolyline) != entityGroups.GetType() && typeof(DxfMText) != entityGroups.GetType() && typeof(DxfText) != entityGroups.GetType() && typeof(DxfCircle) != entityGroups.GetType() && typeof(DxfHatch) != entityGroups.GetType() && typeof(DxfInsert) != entityGroups.GetType() && typeof(DxfSpline) != entityGroups.GetType()) {
                   // Console.WriteLine(entityGroups.GetType());
                }

                dxfType.Add(entityGroups.GetType().Name);
                if (typeof(DxfLine) == entityGroups.GetType())
                {
                    dxfLine = entityGroups as DxfLine;

                    int color;

                    if (dxfLine.Color.ColorType.ToString() == "ByLayer")
                    {
                        color = dxfLine.Layer.Color.Rgb;
                    }
                    else
                    {
                        color = dxfLine.Color.Rgb;
                    }


                    CADLine cADLine = new CADLine
                    {
                        parentHandle = dxfLine.OwnerObjectSoftReference.Handle,
                        type = dxfLine.GetType().Name,
                        color = color,
                        transform = dxfLine.Transform.DebugString,
                        startPoint = dxfLine.Start.ToString(),
                        endPoint = dxfLine.End.ToString(),
                        lineWeight = dxfLine.LineWeight
                    };

                    cadEntities.Add(cADLine);
                }


                if (typeof(DxfLwPolyline) == entityGroups.GetType())
                {
                    //Console.WriteLine(dxfLwPolyline);

                    dxfLwPolyline = entityGroups as DxfLwPolyline;
                    
                    string[] arrVertices = new string[dxfLwPolyline.Vertices.Count];
                    for (int i = 0; i < dxfLwPolyline.Vertices.Count; i++)
                    {
                        arrVertices[i] = dxfLwPolyline.Vertices[i].ToString();
                    }

                    int color;
                    if (dxfLwPolyline.Color.ColorType.ToString() == "ByLayer")
                    {
                        color = dxfLwPolyline.Layer.Color.Rgb;
                    }
                    else
                    {
                        color = dxfLwPolyline.Color.Rgb;
                    }


                    CADLwPolyLine cADLwPolyLine = new CADLwPolyLine
                    {
                        parentHandle = dxfLwPolyline.OwnerObjectSoftReference.Handle,
                        type = dxfLwPolyline.GetType().Name,
                        color = color,
                        transform = dxfLwPolyline.Transform.DebugString,

                        vertices = arrVertices,
                        closed = dxfLwPolyline.Closed.ToString(),
                    };

                    cadEntities.Add(cADLwPolyLine);



                }


                if (typeof(DxfXLine) == entityGroups.GetType())
                {

                    dxfXLine = entityGroups as DxfXLine;

                }

                if (typeof(DxfSpline) == entityGroups.GetType())
                {

                    dxfSpline = entityGroups as DxfSpline;

                    string[] arrFitPoints = new string[dxfSpline.FitPoints.Count];
                    for (int i = 0; i < dxfSpline.FitPoints.Count; i++)
                    {
                        arrFitPoints[i] = dxfSpline.FitPoints[i].ToString();
                    }

                    int color;
                    if (dxfSpline.Color.ColorType.ToString() == "ByLayer")
                    {
                        color = dxfSpline.Layer.Color.Rgb;
                    }
                    else
                    {
                        color = dxfSpline.Color.Rgb;
                    }


                    CADSpline cADSpline = new CADSpline
                    {
                        parentHandle = dxfSpline.OwnerObjectSoftReference.Handle,
                        type = dxfSpline.GetType().Name,
                        color = color,
                        transform = dxfSpline.Transform.DebugString,

                        fitPoints = arrFitPoints,
                    };

                    cadEntities.Add(cADSpline);

                }


                if (typeof(DxfCircle) == entityGroups.GetType())
                {



                    dxfCircle = entityGroups as DxfCircle;

                    int color;
                    if (dxfCircle.Color.ColorType.ToString() == "ByLayer")
                    {
                        color = dxfCircle.Layer.Color.Rgb;
                    }
                    else {
                        color = dxfCircle.Color.Rgb;
                    }


                    CADCircle cADCircle = new CADCircle
                    {
                        parentHandle = dxfCircle.OwnerObjectSoftReference.Handle,
                        type = dxfCircle.GetType().Name,
                        color = color,
                        transform = dxfCircle.Transform.DebugString,

                        center = dxfCircle.Center.ToString(),
                        radius = dxfCircle.Radius
                    };

                    cadEntities.Add(cADCircle);

                    kcircle.Add(dxfCircle);

                }

                if (typeof(DxfArc) == entityGroups.GetType())
                {
                    dxfArc = entityGroups as DxfArc;

                    int color;
                    if (dxfArc.Color.ColorType.ToString() == "ByLayer")
                    {
                        color = dxfArc.Layer.Color.Rgb;
                    }
                    else
                    {
                        color = dxfArc.Color.Rgb;
                    }

                    CADArc cADArc = new CADArc
                    {
                        parentHandle = dxfArc.OwnerObjectSoftReference.Handle,
                        type = dxfArc.GetType().Name,
                        color = color,
                        transform = dxfArc.Transform.DebugString,

                        center = dxfArc.Center.ToString(),
                        startAngle = dxfArc.StartAngle,
                        endAngle = dxfArc.EndAngle,
                        radius = dxfArc.Radius

                    };

                    cadEntities.Add(cADArc);



                }

                if (typeof(DxfEllipse) == entityGroups.GetType())
                {
                    dxfEllipse = entityGroups as DxfEllipse;

                    int color;
                    if (dxfEllipse.Color.ColorType.ToString() == "ByLayer")
                    {
                        color = dxfEllipse.Layer.Color.Rgb;
                    }
                    else
                    {
                        color = dxfEllipse.Color.Rgb;
                    }

                    CADEllipse cADEllipse = new CADEllipse
                    {
                        parentHandle = dxfEllipse.OwnerObjectSoftReference.Handle,
                        type = dxfEllipse.GetType().Name,
                        color = color,
                        transform = dxfEllipse.Transform.DebugString,

                        center = dxfEllipse.Center.ToString(),
                        majorAxisEndPoint = dxfEllipse.MajorAxisEndPoint.ToString(),
                        minorAxisEndPoint = dxfEllipse.MinorAxisEndPoint.ToString(),
                        startParameter = dxfEllipse.StartParameter,
                        endParameter = dxfEllipse.EndParameter

                    };

                    cadEntities.Add(cADEllipse);


                }



                if (typeof(DxfMText) == entityGroups.GetType())
                {
                    dxfMText = entityGroups as DxfMText;

                    int color;
                    if (dxfMText.Color.ColorType.ToString() == "ByLayer")
                    {
                        color = dxfMText.Layer.Color.Rgb;
                    }
                    else
                    {
                        color = dxfMText.Color.Rgb;
                    }


                    if (dxfMText.SimplifiedText == "C座一层平面图")
                    {
                        ktls.Add(dxfMText);
                    }


                    CADMText cADMText = new CADMText
                    {
                        parentHandle = dxfMText.OwnerObjectSoftReference.Handle,
                        type = dxfMText.GetType().Name,
                        color = color,
                        transform = dxfMText.Transform.DebugString,

                        simplifiedText = dxfMText.SimplifiedText.ToString(),
                        fontStyle = dxfMText.Style.ToString(),
                        size = dxfMText.Height,

                        attachmentPoint = dxfMText.AttachmentPoint.ToString(),
                        boxHeight = dxfMText.BoxHeight,
                        boxWidth = dxfMText.BoxWidth
                    };

                    cadEntities.Add(cADMText);

                    if (dxfMText.SimplifiedText.ToString() == "图 纸 目 录")
                    {
                        //Console.WriteLine(222222);
                        ktls.Add(dxfMText);

                    }
                    if (dxfMText.SimplifiedText.ToString() == "JS-T5-302")
                    {
                        //Console.WriteLine(222222);
                        ktls.Add(dxfMText);

                    }



                }


                if (typeof(DxfText) == entityGroups.GetType())
                {
                    dxfText = entityGroups as DxfText;

                    int color;
                    if (dxfText.Color.ColorType.ToString() == "ByLayer")
                    {
                        color = dxfText.Layer.Color.Rgb;
                    }
                    else
                    {
                        color = dxfText.Color.Rgb;
                    }

                    CADText cADText = new CADText
                    {
                        parentHandle = dxfText.OwnerObjectSoftReference.Handle,
                        type = dxfText.GetType().Name,
                        color = color,
                        transform = dxfText.Transform.DebugString,

                        simplifiedText = dxfText.SimplifiedText.ToString(),
                        fontStyle = dxfText.Style.ToString(),
                        size = dxfText.Height,

                        alignMentPoint1 = dxfText.AlignmentPoint1.ToString(),
                        rotationAngle = dxfText.Rotation,
                    };


                    if (dxfText.SimplifiedText == "JS-T5-001 ")
                    {
                        ktest.Add(dxfText);
                    }

                    cadEntities.Add(cADText);

                }

                if (typeof(DxfInsert) == entityGroups.GetType())
                {

                    dxfInsert = entityGroups as DxfInsert;
                    kins.Add(dxfInsert);


                    if (dxfInsert.Block != null)
                    {

                        ulong count;
                        if (dxfInsert.Block != null)
                        {
                            count = dxfInsert.Block.Handle;

                        }
                        else
                        {
                            count = 0;
                        }

                        CADInsert cADInsert = new CADInsert();


                        cADInsert.parentHandle = dxfInsert.OwnerObjectSoftReference.Handle; 
                        cADInsert.type = dxfInsert.GetType().Name;
                        cADInsert.transform = dxfInsert.Transform.DebugString;

                        cADInsert.insertPoint = dxfInsert.InsertionPoint.ToString();
                        cADInsert.rotationAngle = dxfInsert.Rotation;
                        cADInsert.insertName = dxfInsert.Block.Name;
                        cADInsert.nowHandle = count;
                        cADInsert.insertScale = dxfInsert.ScaleFactor.ToString();

                        cadEntities.Add(cADInsert);

                        FindEntities(dxfInsert.Block.Entities);
                    }

                }



                if (typeof(DxfAttributeDefinition) == entityGroups.GetType())
                {
                    dxfAttributeDefinition = entityGroups as DxfAttributeDefinition;
                    //Console.WriteLine(dxfAttributeDefinition);

                }

                if (typeof(DxfDimension.Linear) == entityGroups.GetType()) {
                    Console.WriteLine(11111);
                }
            }

        }




        public static void Main(string[] args)
        {


            if (args.Length != 2)
            {
                args = new string[2];
                //args[1] = "D:\\C项目\\CadLCmd\\CadLCmd\\dwg\\test1.dwg";
                //args[1] = "D:\\C项目\\CadLCmd\\CadLCmd\\dwg\\test2.dwg";
                //args[1] = "D:\\C项目\\CadLCmd\\CadLCmd\\dwg\\test3.dwg";
                //args[1] = "D:\\C项目\\CadLCmd\\CadLCmd\\dwg\\test4.dwg";
                args[1] = "D:\\C项目\\CadLCmd\\CadLCmd\\dwg\\test5Insert.dwg";
                //args[1] = "D:\\C项目\\CadLCmd\\CadLCmd\\dwg\\test6.dwg";
            }
            string format = args[0];
            string filename = args[1];

            DxfModel model = null;
            try
            {
                string extension = Path.GetExtension(filename);
                if (string.Compare(extension, ".dwg", true) == 0)
                {
                    model = DwgReader.Read(filename);
                }
                else
                {
                    model = DxfReader.Read(filename);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error occurred: " + e.Message);
                Environment.Exit(1);
            }

            //foreach (var entityGroups in model.Entities.GroupBy(a => a.GetType()))
            //{

            //    Console.WriteLine(entityGroups.GetType());

            //    //if (typeof(DxfLine) == entityGroups.Key)
            //    //{
            //    //    foreach (var item in entityGroups)
            //    //    {
            //    //        Console.WriteLine(item.Color);

            //    //    }
            //    //}

            //}
            //FileStream fs = new FileStream("D:\\C项目\\CadLCmd\\CadLCmd\\txt\\test1.txt", FileMode.Create);



            FindEntities(model.Entities);



            string json = JsonConvert.SerializeObject(cadEntities);
            File.WriteAllText("D:\\C项目\\CadLCmd\\CadLCmd\\txt\\model.json", json);
            File.WriteAllText("D:\\Project\\cadtest\\node_modules\\@cadTestUbim\\res\\data\\dxfdata.json", json);

            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();

            KTCodes = ktls.ToArray();

            KTest = ktest.ToArray();

            DxfType = dxfType.ToArray();

            KInsert = kins.ToArray();

            KEC = kco.ToArray();

            KCIRCLE = kcircle.ToArray();

            //Console.ReadKey();
            string outfile = Path.GetDirectoryName(Path.GetFullPath(filename)) + "\\12";
            Stream stream;
            if (format == "pdf")
            {
                BoundsCalculator boundsCalculator = new BoundsCalculator();
                boundsCalculator.GetBounds(model);
                Bounds3D bounds = boundsCalculator.Bounds;
                PaperSize paperSize = PaperSizes.GetPaperSize(PaperKind.Letter);
                // Lengths in inches. 
                float pageWidth = (float)paperSize.Width / 100f;
                float pageHeight = (float)paperSize.Height / 100f;
                float margin = 0.5f;
                // Scale and transform such that its fits max width/height 
                // and the top left middle of the cad drawing will match the  
                // top middle of the pdf page. 
                // The transform transforms to pdf pixels.
                Matrix4D to2DTransform = DxfUtil.GetScaleTransform(
                    bounds.Corner1,
                    bounds.Corner2,
                    new Point3D(bounds.Center.X, bounds.Corner2.Y, 0d),
                    new Point3D(new Vector3D(margin, margin, 0d) * PdfExporter.InchToPixel),
                    new Point3D(new Vector3D(pageWidth - margin, pageHeight - margin, 0d) * PdfExporter.InchToPixel),
                    new Point3D(new Vector3D(pageWidth / 2d, pageHeight - margin, 0d) * PdfExporter.InchToPixel)
                );
                using (stream = File.Create(outfile + ".pdf"))
                {
                    PdfExporter pdfGraphics = new PdfExporter(stream);
                    pdfGraphics.DrawPage(
                        model,
                        GraphicsConfig.WhiteBackgroundCorrectForBackColor,
                        to2DTransform,
                        paperSize
                    );
                    pdfGraphics.EndDocument();
                }
            }
            else
            {
                GDIGraphics3D graphics = new GDIGraphics3D(GraphicsConfig.BlackBackgroundCorrectForBackColor);
                Size maxSize = new Size(500, 500);
                Bitmap bitmap =
                    ImageExporter.CreateAutoSizedBitmap(
                        model,
                        graphics,
                        Matrix4D.Identity,
                        System.Drawing.Color.Black,
                        maxSize
                    );
                switch (format)
                {
                    case "bmp":
                        using (stream = File.Create(outfile + ".bmp"))
                        {
                            ImageExporter.EncodeImageToBmp(bitmap, stream);
                        }
                        break;
                    case "gif":
                        using (stream = File.Create(outfile + ".gif"))
                        {
                            ImageExporter.EncodeImageToGif(bitmap, stream);
                        }
                        break;
                    case "tiff":
                        using (stream = File.Create(outfile + ".tiff"))
                        {
                            ImageExporter.EncodeImageToTiff(bitmap, stream);
                        }
                        break;
                    case "png":
                        using (stream = File.Create(outfile + ".png"))
                        {
                            ImageExporter.EncodeImageToPng(bitmap, stream);
                        }
                        break;
                    case "jpg":
                        using (stream = File.Create(outfile + ".jpg"))
                        {
                            ImageExporter.EncodeImageToJpeg(bitmap, stream);
                        }
                        break;
                    default:
                        Console.WriteLine("Unknown format " + format + ".");
                        break;
                }
            }
        }
        /// <summary> 
        /// 将 Stream 写入文件 
        /// </summary> 
        public static void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[] 
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件 
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }
    }
}

