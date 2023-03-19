using System.Runtime.InteropServices;
using UnityEngine;

public static class OpenFileWin32
{
    public static string OpenFile()
    {
        var dialog = new OpenFile();
        dialog.structSize = Marshal.SizeOf(dialog);
        dialog.filter = "fbx files\0*.fbx\0obj files\0*.obj\0All Files\0*.*\0\0";
        dialog.file = new string(new char[256]);
        dialog.maxFile = dialog.file.Length;
        dialog.fileTitle = new string(new char[64]);
        dialog.maxFileTitle = dialog.fileTitle.Length;
        dialog.initialDir = Application.dataPath;  //默认路径
        dialog.title = "Open File Dialog";
 
        dialog.defExt = "fbx";//显示文件的类型
        //注意一下项目不一定要全选 但是0x00000008项不要缺少
        //OFN_EXPLORER | OFN_FILEMUSTEXIST | OFN_PATHMUSTEXIST | OFN_NOCHANGEDIR
        dialog.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008; 
        return DialogShow.GetOpenFileName(dialog) ? dialog.file : null;
    }
}
