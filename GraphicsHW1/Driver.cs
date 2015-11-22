using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GraphicsHW.Primitives;
using GraphicsHW.Util;
using GraphicsHW.Math;

namespace GraphicsHW
{
    public class Driver
    {
        static void Main(string[] args)
        {
            //Parse the argument array in arbitrary order and give an interface to retrieve them
            Arguments a = new Arguments(args);
            //Read the input file specified in the arguments
            //PostscriptReader rdr = new PostscriptReader(a.InputFile);
            SMFReader rdr = new SMFReader(a.InputFile);
            List<Polygon3D> prims = rdr.ReadFile();
            Matrix4<double> cameraTranslation = new Matrix4<double>
                (
                1, 0, 0, -a.VRP_X,
                0, 1, 0, -a.VRP_Y,
                0, 0, 1, -a.VRP_Z,
                0, 0, 0, 1
                );
            //Cross product
            double VRR_X = a.VUP_Y * a.VPN_Z - a.VUP_Z * a.VPN_Y;
            double VRR_Y = a.VUP_Z * a.VPN_X - a.VUP_X * a.VPN_Z;
            double VRR_Z = a.VUP_X * a.VPN_Y - a.VUP_Y * a.VPN_X;
            double normalizer = System.Math.Sqrt(System.Math.Pow(VRR_X, 2) + System.Math.Pow(VRR_Y, 2) + System.Math.Pow(VRR_Z, 2));
            VRR_X = VRR_X / normalizer;
            VRR_Y = VRR_Y / normalizer;
            VRR_Z = VRR_Z / normalizer;
            double normalizerVUP = System.Math.Sqrt(System.Math.Pow(a.VUP_X, 2) + System.Math.Pow(a.VUP_Y, 2) + System.Math.Pow(a.VUP_Z, 2));
            double VUP_X = a.VUP_X / normalizerVUP;
            double VUP_Y = a.VUP_Y / normalizerVUP;
            double VUP_Z = a.VUP_Z / normalizerVUP;
            double normalizerVPN = System.Math.Sqrt(System.Math.Pow(a.VPN_X, 2) + System.Math.Pow(a.VPN_Y, 2) + System.Math.Pow(a.VPN_Z, 2));
            double VPN_X = a.VPN_X / normalizerVPN;
            double VPN_Y = a.VPN_Y / normalizerVPN;
            double VPN_Z = a.VPN_Z / normalizerVPN;
            Matrix4<double> cameraRotation = new Matrix4<double>
                (
                VRR_X, VRR_Y, VRR_Z, 0,
                VUP_X, VUP_Y, VUP_Z, 0,
                VPN_X, VPN_Y, VPN_Z, 0,
                0, 0, 0, 1
                );

            Matrix4<double> projection;
            if (a.IsParallelProjection)
            {
                projection = Matrix4<double>.GetParallelProjectionMatrix(a.VRC_UMIN, a.VRC_UMAX, a.VRC_VMIN, a.VRC_VMAX, a.PRP_X, a.PRP_Y, a.PRP_Z, 0.6, -0.6);
                //projection *= Matrix4<double>.GetPerspectiveProjectionMatrix(a.VRC_UMIN, a.VRC_UMAX, a.VRC_VMIN, a.VRC_VMAX, a.PRP_X, a.PRP_Y, a.PRP_Z, -0.6);
            }
            else
            {
                projection = Matrix4<double>.GetPerspectiveProjectionMatrix(a.VRC_UMIN, a.VRC_UMAX, a.VRC_VMIN, a.VRC_VMAX, a.PRP_X, a.PRP_Y, a.PRP_Z, -0.6);
                //projection *= Matrix4<double>.GetParallelProjectionMatrix(a.VRC_UMIN, a.VRC_UMAX, a.VRC_VMIN, a.VRC_VMAX, a.PRP_X, a.PRP_Y, a.PRP_Z, 0.6, -0.6);
            }
            foreach (var p in prims)
            {
                //Transfrom into canonical orthogonal view volume to perform clipping?? Tried it and didn't get correct results. But this doesn't work either so something else must be wrong...
                p.ProjectAndView(cameraTranslation);
                p.ProjectAndView(cameraRotation);
                p.ProjectAndView(projection);
            }
            List<Polygon3D> acceptedPolys = new List<Polygon3D>();
            //Clip polygons
            foreach (var p in prims)
            {
                bool accept = false;
                foreach (var v in p)
                {
                    if (v.X > 1 || v.X < -1)
                    {
                        accept = false;
                    }
                    else if (v.Y > 1 || v.Y < -1)
                    {
                        accept = false;
                    }
                    else
                    {
                        accept = true;
                    }
                }

                if (accept)
                    acceptedPolys.Add(p);
            }
            //This is the part that I think is wrong but i'm not sure how to correct it. The scaling seems to be fine for parallel but not for perspective and the translation
            //is definitely wrong but i'm not sure what it should be. I have these as separate statements instead of matrix multiplication to make it easier to debug
            foreach (var p in acceptedPolys)
            {
                if (a.IsParallelProjection)
                {
                    foreach (var v in p)
                    {
                        v.Z = 0 ;
                        v.X += 1;
                        v.Y += 1;
                        v.X *= (a.XUpper - a.XLower)/2;
                        v.Y *= (a.YUpper - a.YLower)/2;
                        v.X += a.VRC_UMIN;
                        v.Y += a.VRC_VMIN;
                    }
                }
                else
                {
                    //p.ProjectAndView(projection);
                    foreach (var v in p)
                    {
                        //Perspective division
                        v.X /= -v.Z;
                        v.Y /= -v.Z;
                        v.X += 1;
                        v.Y += 1;
                        v.X *= (a.VP_XUpper - a.VP_XLower) / 2;
                        v.Y *= (a.VP_YUpper - a.VP_YLower) / 2;
                        v.X += a.VRC_UMIN;
                        v.Y += a.VRC_VMIN;

                    }
                }

            }
            foreach (var p in acceptedPolys)
            {
                p.MapToViewPort(PixelBuffer.GetVPMatrix(a.XLower, a.XUpper, a.YLower, a.YUpper, a.VP_XLower, a.VP_XUpper, a.VP_YLower, a.VP_YUpper));
            }
            ////Draw lines
            PixelBuffer pb = new PixelBuffer(a.XLower, a.XUpper, a.YLower, a.YUpper, a.VP_XLower, a.VP_XUpper, a.VP_YLower, a.VP_YUpper);
            pb.DrawPolygon3D(acceptedPolys);
            Console.Write(pb.WriteToXPM());
        }
    }
}
