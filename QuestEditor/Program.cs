// Copyright 2019 RED Software, LLC. All Rights Reserved.

using System;
using System.Windows.Forms;

namespace QuestEditor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
