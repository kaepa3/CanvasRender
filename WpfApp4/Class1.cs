﻿//
// File generated by HDevelop for HALCON/.NET (C#) Version 13.0
//

using HalconDotNet;

public partial class HDevelopExport
{
#if !(NO_EXPORT_MAIN || NO_EXPORT_APP_MAIN)
    public HDevelopExport()
    {
        // Default settings used in HDevelop 
        action();
    }
#endif

#if !NO_EXPORT_MAIN
    // Main procedure 
    private void action()
    {


        // Local iconic variables 

        HObject ho_Image, ho_Region;

        // Local control variables 

        HTuple hv_ExpDefaultCtrlDummyVar = null, hv_Width = null;
        HTuple hv_Height = null, hv_WindowHandle = null;
        // Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(out ho_Image);
        HOperatorSet.GenEmptyObj(out ho_Region);
        if (HDevWindowStack.IsOpen())
        {
            HOperatorSet.CloseWindow(HDevWindowStack.Pop());
        }
        ho_Image.Dispose();
        HOperatorSet.ReadImage(out ho_Image, "fabrik");
        HOperatorSet.GetImagePointer1(ho_Image, out hv_ExpDefaultCtrlDummyVar, out hv_ExpDefaultCtrlDummyVar,
            out hv_Width, out hv_Height);
        HOperatorSet.SetWindowAttr("background_color", "black");
        HOperatorSet.OpenWindow(0, 0, hv_Width, hv_Height, 0, "visible", "", out hv_WindowHandle);
        HDevWindowStack.Push(hv_WindowHandle);
        if (HDevWindowStack.IsOpen())
        {
            HOperatorSet.DispObj(ho_Image, HDevWindowStack.GetActive());
        }
        ho_Region.Dispose();
        HOperatorSet.Threshold(ho_Image, out ho_Region, 50, 100);
        HOperatorSet.WriteString(hv_WindowHandle, "hello");
        HOperatorSet.DumpWindow(hv_WindowHandle, "bmp", "halcon_dump");
        ho_Image.Dispose();
        ho_Region.Dispose();

    }

#endif


}