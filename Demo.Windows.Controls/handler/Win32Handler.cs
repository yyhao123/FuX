using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Windows.Controls.handler
{
    public static class Win32Handler
    {
        [ComImport]
        [Guid("D57C7288-D4AD-4768-BE02-9D969532D960")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFileOpenDialog
        {
            [PreserveSig]
            int Show(nint parent);

            void SetFileTypes(uint cFileTypes, [MarshalAs(UnmanagedType.LPArray)] COMDLG_FILTERSPEC[] rgFilterSpec);

            void SetFileTypeIndex();

            void GetFileTypeIndex();

            void Advise();

            void Unadvise();

            void SetOptions(uint options);

            void GetOptions(out uint options);

            void SetDefaultFolder();

            void SetFolder();

            void GetFolder();

            void GetCurrentSelection();

            void SetFileName();

            void GetFileName();

            void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string title);

            void SetOkButtonLabel();

            void SetFileNameLabel();

            void GetResult([MarshalAs(UnmanagedType.Interface)] out IShellItem item);

            void AddPlace();

            void SetDefaultExtension();

            void Close(int hr);

            void SetClientGuid();

            void ClearClientData();

            void SetFilter();
        }

        [ComImport]
        [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItem
        {
            void BindToHandler();

            void GetParent();

            void GetDisplayName(uint sigdnName, out nint ppszName);

            void GetAttributes();

            void Compare();
        }

        [ComImport]
        [Guid("DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7")]
        private class FileOpenDialog
        {
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct COMDLG_FILTERSPEC
        {
            public string pszName;

            public string pszSpec;

            public COMDLG_FILTERSPEC(string name, string spec)
            {
                pszName = name;
                pszSpec = spec;
            }
        }

        private const uint FOS_PICKFOLDERS = 32u;

        private const uint FOS_FORCEFILESYSTEM = 64u;

        private const uint SIGDN_FILESYSPATH = 2147844096u;

        public static string Select(string title, bool selectFolder = false, Dictionary<string, string>? filters = null)
        {
            IFileOpenDialog fileOpenDialog = (IFileOpenDialog)new FileOpenDialog();
            uint num = 64u;
            if (selectFolder)
            {
                num |= 0x20u;
            }

            fileOpenDialog.SetOptions(num);
            fileOpenDialog.SetTitle(title);
            if (!selectFolder && filters != null && filters.Count > 0)
            {
                COMDLG_FILTERSPEC[] array = filters.Select<KeyValuePair<string, string>, COMDLG_FILTERSPEC>((KeyValuePair<string, string> f) => new COMDLG_FILTERSPEC(f.Key, f.Value)).ToArray();
                fileOpenDialog.SetFileTypes((uint)array.Length, array);
            }

            if (fileOpenDialog.Show(IntPtr.Zero) != 0)
            {
                return string.Empty;
            }

            fileOpenDialog.GetResult(out IShellItem item);
            item.GetDisplayName(2147844096u, out var ppszName);
            string? result = Marshal.PtrToStringUni(ppszName);
            Marshal.FreeCoTaskMem(ppszName);
            return result;
        }
    }
}
