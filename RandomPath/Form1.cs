using System;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using Tulpep.NotificationWindow;


namespace RandomPath
{
    public partial class Form1 : Form
    {
        private IKeyboardMouseEvents m_GlobalHook;

        private bool active = false;
        
        private Random r = new Random();

        private NumericUpDown[] nuds = new NumericUpDown[9];

        public Form1() {
            InitializeComponent();
            
            nuds = new NumericUpDown[] {
                numericUpDown0,
                numericUpDown1,
                numericUpDown2,
                numericUpDown3,
                numericUpDown4,
                numericUpDown5,
                numericUpDown6,
                numericUpDown7,
                numericUpDown8
        };

            Subscribe();
        }

        public void Subscribe() {
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            m_GlobalHook.KeyDown += GlobalHookKeyDown;
        }

        private void GlobalHookKeyDown(object sender, KeyEventArgs e) {
            //Console.WriteLine("KeyPress: \t{0}",e.KeyValue);
            if(e.KeyValue == 119) {
                active = !active;
                notify("RandomPath is now " + (active ? "" : "in") + "active.");
            }
        }
        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e) {
            if(e.Button == MouseButtons.Right && active) {
                int block = randomBlock() + 1;
                Console.Write(block);
                SendKeys.Send("" + block);
            }
        }

        private int randomBlock() {
            decimal chance = 0;
            foreach (NumericUpDown nud in nuds) { chance += nud.Value;}
            int pos = (int)(r.NextDouble() * ((double)chance));
            
            for (int i = 0; i < 9; i++) {
                NumericUpDown nud = nuds[i];
                pos -= (int)nud.Value;

                if(pos < 0) {
                    return i;
                }
            }
            return -1;

        }
        
        private void notify(String msg) {
            var popupNotifier = new PopupNotifier();
            popupNotifier.TitleText = "RandomPath";
            popupNotifier.ContentText = msg;
            popupNotifier.IsRightToLeft = false;
            popupNotifier.Popup();
        }

        private void zeroAll_Click(object sender, EventArgs e) {
            foreach(NumericUpDown nud in nuds) {
                nud.Value = 0;
            }
        }
        
    }
}
