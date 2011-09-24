using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Imaging;
using AForge.Imaging;
using System.Threading;
using AutoMouse;

namespace FeedPuzzler
{
    public partial class frmFeedPuzzler_Crop : Form
    {
        public frmFeedPuzzler_Crop()
        {
            InitializeComponent();
        }

        public void SetManual(bool value)
        {
            isManual = value;
            UpdateUI();
        }

        public void UpdateUI()
        {
            DishText_1_1.Visible = isManual;
            DishText_1_2.Visible = isManual;
            DishText_1_3.Visible = isManual;

            DishText_2_1.Visible = isManual;
            DishText_2_2.Visible = isManual;
            DishText_2_3.Visible = isManual;

            DishText_3_1.Visible = isManual;
            DishText_3_2.Visible = isManual;
            DishText_3_3.Visible = isManual;

            SpoonText1.Visible = isManual;
            SpoonText2.Visible = isManual;
            SpoonText3.Visible = isManual;
        }

        public bool _IsForceDoubleClick;

        string[] steps = null;
        int _DelayMs = 500;
        bool isManual = false;

        Size _TargetBlockSize = new Size(220, 90);
        Size _SpoonBlockSize = new Size(100, 80);
        Size _SupplyBlockSize = new Size(100, 90);

        int _TargetBlockBlankHeight = 10;//ระยะห่างระหว่าง targetblock ในแนวตั้ง
        int _TargetBlockToSpoonBlockHeight = -6;//จาก Target มา spoon มันเหลือมกันอยู่ 6 (spoon อยู่สูงกว่า)
        int _TargetBlockToSpoonBlockWidth = 78;

        int _SpoonBlockBlankHeight = 20;
        int _SpoonBlockToSupplylockHeight = 9;
        int _SpoonBlockToSupplylockWidth = 75;

        int _SupplyBlockBlankHeight = 10;

        //Text in Block
        //Target
        Size _Target_SupplyType = new Size(14, 17);

        int _Target_SupplyType_BlankWidth = 101;
        int _Target_SupplyType_BlankHeight = 10;

        Size _Target_SupplyNumber = new Size(11, 17);
        int _Target_SupplyNumber_BlickWidth = 139;


        //Spoon
        Size _SpoonSize_Size = new Size(11, 19);
        int _SpoonSize_BlankWidth = 78;
        int _SpoonSize_BlankHeight = 57;

        //Supply


        //SupplyTypeImage
        Bitmap[] _TargetTypeImages = new Bitmap[4];//A B C + ว่าง

        //TargetSizeImage
        Bitmap[] _TargetSizeImages = new Bitmap[10];// 1 - 9  + ว่าง

        //SpoonSizeImage
        Bitmap[] _SpoonSizeImages = new Bitmap[8];//ขาด 1 ไม่มี

        private void frmFeedPuzzler_Crop_Load(object sender, EventArgs e)
        {
            UpdateUI();

            Gma.UserActivityMonitor.HookManager.MouseDown += HookManager_MouseDown;
            Gma.UserActivityMonitor.HookManager.KeyDown += HookManager_KeyDown;

            //targetblock
            pnTarget1.Size = pnTarget2.Size = pnTarget3.Size = _TargetBlockSize;

            pnTarget2.Location = new Point(pnTarget1.Location.X, pnTarget1.Location.Y + _TargetBlockSize.Height + _TargetBlockBlankHeight);
            pnTarget3.Location = new Point(pnTarget1.Location.X, pnTarget2.Location.Y + _TargetBlockSize.Height + _TargetBlockBlankHeight);
        
            //spoon
            pnSpoon1.Size = pnSpoon2.Size = pnSpoon3.Size = _SpoonBlockSize;

            pnSpoon1.Location = new Point(pnTarget1.Location.X + _TargetBlockSize.Width + _TargetBlockToSpoonBlockWidth, pnTarget1.Location.Y + _TargetBlockToSpoonBlockHeight);
            pnSpoon2.Location = new Point(pnSpoon1.Location.X, pnSpoon1.Location.Y + _SpoonBlockSize.Height + _SpoonBlockBlankHeight);
            pnSpoon3.Location = new Point(pnSpoon2.Location.X, pnSpoon2.Location.Y + _SpoonBlockSize.Height + _SpoonBlockBlankHeight);
            
            //supply
            pnSupply1.Size = pnSupply2.Size = pnSupply3.Size = _SupplyBlockSize;

            pnSupply1.Location = new Point(pnSpoon1.Location.X + _SupplyBlockSize.Width + _SpoonBlockToSupplylockWidth, pnSpoon1.Location.Y + _SpoonBlockToSupplylockHeight);
            pnSupply2.Location = new Point(pnSupply1.Location.X, pnSupply1.Location.Y + _SupplyBlockSize.Height + _SupplyBlockBlankHeight);
            pnSupply3.Location = new Point(pnSupply2.Location.X, pnSupply2.Location.Y + _SupplyBlockSize.Height + _SupplyBlockBlankHeight);
        
            
            //Text in Target
            //1
            pnTargetSupplyType1_1.Size = pnTargetSupplyType1_2.Size = pnTargetSupplyType1_3.Size = _Target_SupplyType;
            
            pnTargetSupplyType1_1.Location = new Point(_Target_SupplyType_BlankWidth, _Target_SupplyType_BlankHeight);
            pnTargetSupplyType1_2.Location = new Point(_Target_SupplyType_BlankWidth, pnTargetSupplyType1_1.Location.Y + _Target_SupplyType.Height + _Target_SupplyType_BlankHeight);
            pnTargetSupplyType1_3.Location = new Point(_Target_SupplyType_BlankWidth, pnTargetSupplyType1_2.Location.Y + _Target_SupplyType.Height + _Target_SupplyType_BlankHeight);

            pnTargetSupplyNumber1_1.Size = pnTargetSupplyNumber1_2.Size = pnTargetSupplyNumber1_3.Size = _Target_SupplyNumber;

            pnTargetSupplyNumber1_1.Location = new Point(_Target_SupplyNumber_BlickWidth, _Target_SupplyType_BlankHeight);
            pnTargetSupplyNumber1_2.Location = new Point(_Target_SupplyNumber_BlickWidth, pnTargetSupplyNumber1_1.Location.Y + _Target_SupplyNumber.Height + _Target_SupplyType_BlankHeight);
            pnTargetSupplyNumber1_3.Location = new Point(_Target_SupplyNumber_BlickWidth, pnTargetSupplyNumber1_2.Location.Y + _Target_SupplyNumber.Height + _Target_SupplyType_BlankHeight);

            //2
            pnTargetSupplyType2_1.Size = pnTargetSupplyType2_2.Size = pnTargetSupplyType2_3.Size = _Target_SupplyType;

            pnTargetSupplyType2_1.Location = new Point(_Target_SupplyType_BlankWidth, _Target_SupplyType_BlankHeight);
            pnTargetSupplyType2_2.Location = new Point(_Target_SupplyType_BlankWidth, pnTargetSupplyType2_1.Location.Y + _Target_SupplyType.Height + _Target_SupplyType_BlankHeight);
            pnTargetSupplyType2_3.Location = new Point(_Target_SupplyType_BlankWidth, pnTargetSupplyType2_2.Location.Y + _Target_SupplyType.Height + _Target_SupplyType_BlankHeight);

            pnTargetSupplyNumber2_1.Size = pnTargetSupplyNumber2_2.Size = pnTargetSupplyNumber2_3.Size = _Target_SupplyNumber;

            pnTargetSupplyNumber2_1.Location = new Point(_Target_SupplyNumber_BlickWidth, _Target_SupplyType_BlankHeight);
            pnTargetSupplyNumber2_2.Location = new Point(_Target_SupplyNumber_BlickWidth, pnTargetSupplyNumber2_1.Location.Y + _Target_SupplyNumber.Height + _Target_SupplyType_BlankHeight);
            pnTargetSupplyNumber2_3.Location = new Point(_Target_SupplyNumber_BlickWidth, pnTargetSupplyNumber2_2.Location.Y + _Target_SupplyNumber.Height + _Target_SupplyType_BlankHeight);

            //3
            pnTargetSupplyType3_1.Size = pnTargetSupplyType3_2.Size = pnTargetSupplyType3_3.Size = _Target_SupplyType;

            pnTargetSupplyType3_1.Location = new Point(_Target_SupplyType_BlankWidth, _Target_SupplyType_BlankHeight);
            pnTargetSupplyType3_2.Location = new Point(_Target_SupplyType_BlankWidth, pnTargetSupplyType3_1.Location.Y + _Target_SupplyType.Height + _Target_SupplyType_BlankHeight);
            pnTargetSupplyType3_3.Location = new Point(_Target_SupplyType_BlankWidth, pnTargetSupplyType3_2.Location.Y + _Target_SupplyType.Height + _Target_SupplyType_BlankHeight);

            pnTargetSupplyNumber3_1.Size = pnTargetSupplyNumber3_2.Size = pnTargetSupplyNumber3_3.Size = _Target_SupplyNumber;

            pnTargetSupplyNumber3_1.Location = new Point(_Target_SupplyNumber_BlickWidth, _Target_SupplyType_BlankHeight);
            pnTargetSupplyNumber3_2.Location = new Point(_Target_SupplyNumber_BlickWidth, pnTargetSupplyNumber3_1.Location.Y + _Target_SupplyNumber.Height + _Target_SupplyType_BlankHeight);
            pnTargetSupplyNumber3_3.Location = new Point(_Target_SupplyNumber_BlickWidth, pnTargetSupplyNumber3_2.Location.Y + _Target_SupplyNumber.Height + _Target_SupplyType_BlankHeight);
        
            //Text in Spoon
            pnSpoonSize1.Size = pnSpoonSize2.Size = pnSpoonSize3.Size = _SpoonSize_Size;

            pnSpoonSize1.Location = new Point(_SpoonSize_BlankWidth, _SpoonSize_BlankHeight);
            pnSpoonSize2.Location = new Point(_SpoonSize_BlankWidth, _SpoonSize_BlankHeight);
            pnSpoonSize3.Location = new Point(_SpoonSize_BlankWidth, _SpoonSize_BlankHeight);
        

            //Prepare Image for Detect
            //TargetType
            _TargetTypeImages[0] = new Bitmap(Application.StartupPath + "/Images/TargetA.png");
            _TargetTypeImages[1] = new Bitmap(Application.StartupPath + "/Images/TargetB.png");
            _TargetTypeImages[2] = new Bitmap(Application.StartupPath + "/Images/TargetC.png");
            _TargetTypeImages[3] = new Bitmap(Application.StartupPath + "/Images/TargetEmpty.png");//ว่าง

            //TargetSize
            _TargetSizeImages[0] = new Bitmap(Application.StartupPath + "/Images/Target1.png");
            _TargetSizeImages[1] = new Bitmap(Application.StartupPath + "/Images/Target2.png");
            _TargetSizeImages[2] = new Bitmap(Application.StartupPath + "/Images/Target3.png");
            _TargetSizeImages[3] = new Bitmap(Application.StartupPath + "/Images/Target4.png");
            _TargetSizeImages[4] = new Bitmap(Application.StartupPath + "/Images/Target5.png");
            _TargetSizeImages[5] = new Bitmap(Application.StartupPath + "/Images/Target6.png");
            _TargetSizeImages[6] = new Bitmap(Application.StartupPath + "/Images/Target7.png");
            _TargetSizeImages[7] = new Bitmap(Application.StartupPath + "/Images/Target8.png");
            _TargetSizeImages[8] = new Bitmap(Application.StartupPath + "/Images/Target9.png");
            _TargetSizeImages[9] = new Bitmap(Application.StartupPath + "/Images/Target0.png");//ว่าง

            //SpoonSize
            _SpoonSizeImages[0] = new Bitmap(Application.StartupPath + "/Images/Spoon2.png");
            _SpoonSizeImages[1] = new Bitmap(Application.StartupPath + "/Images/Spoon3.png");
            _SpoonSizeImages[2] = new Bitmap(Application.StartupPath + "/Images/Spoon4.png");
            _SpoonSizeImages[3] = new Bitmap(Application.StartupPath + "/Images/Spoon5.png");
            _SpoonSizeImages[4] = new Bitmap(Application.StartupPath + "/Images/Spoon6.png");
            _SpoonSizeImages[5] = new Bitmap(Application.StartupPath + "/Images/Spoon7.png");
            _SpoonSizeImages[6] = new Bitmap(Application.StartupPath + "/Images/Spoon8.png");
            _SpoonSizeImages[7] = new Bitmap(Application.StartupPath + "/Images/Spoon9.png");
        }

        //Capture
        Bitmap screenshot;
        public void Play(int delayMs)
        {
            this.Hide();
            _DelayMs = delayMs;

            int[] first = new int[] { int.Parse(DishText_1_1.Text), int.Parse(DishText_1_2.Text), int.Parse(DishText_1_3.Text) };
            int[] second = new int[] { int.Parse(DishText_2_1.Text), int.Parse(DishText_2_2.Text), int.Parse(DishText_2_3.Text) };
            int[] third = new int[] { int.Parse(DishText_3_1.Text), int.Parse(DishText_3_2.Text), int.Parse(DishText_3_3.Text) };
            int[] spoons = new int[] { int.Parse(SpoonText1.Text), int.Parse(SpoonText2.Text), int.Parse(SpoonText3.Text) };

            if (!isManual)
            {
                //Cap a screenshot
                screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format24bppRgb);
                Graphics gfxScreenshot = Graphics.FromImage(screenshot);
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);


                
                //Fix 3 SupplyType
                int[] SupplyType = new int[3];
                

                //Indentify Images
                #region TargetType A B C
                int[] tmpTargetType = new int[9];

                //for save bitmap to file
                //Bitmap SpoonSizeTmp;
                //SpoonSizeTmp = this.getPieces(pnTarget1.PointToScreen(pnTargetSupplyNumber1_1.Location).X
                //     , pnTarget1.PointToScreen(pnTargetSupplyNumber1_1.Location).Y
                //     , _Target_SupplyNumber.Width
                //     , _Target_SupplyNumber.Height);
                //SpoonSizeTmp.Save(Guid.NewGuid().ToString() + ".png");

                //return;

                tmpTargetType[0] = this.getMaxSimilarity(this.getPieces(pnTarget1.PointToScreen(pnTargetSupplyType1_1.Location).X
                   , pnTarget1.PointToScreen(pnTargetSupplyType1_1.Location).Y
                   , _Target_SupplyType.Width
                   , _Target_SupplyType.Height), _TargetTypeImages);

                tmpTargetType[1] = this.getMaxSimilarity(this.getPieces(pnTarget1.PointToScreen(pnTargetSupplyType1_2.Location).X
                    , pnTarget1.PointToScreen(pnTargetSupplyType1_2.Location).Y
                    , _Target_SupplyType.Width
                    , _Target_SupplyType.Height), _TargetTypeImages);


                tmpTargetType[2] = this.getMaxSimilarity(this.getPieces(pnTarget1.PointToScreen(pnTargetSupplyType1_3.Location).X
                    , pnTarget1.PointToScreen(pnTargetSupplyType1_3.Location).Y
                    , _Target_SupplyType.Width
                    , _Target_SupplyType.Height), _TargetTypeImages);

                tmpTargetType[3] = this.getMaxSimilarity(this.getPieces(pnTarget2.PointToScreen(pnTargetSupplyType2_1.Location).X
                    , pnTarget2.PointToScreen(pnTargetSupplyType2_1.Location).Y
                    , _Target_SupplyType.Width
                    , _Target_SupplyType.Height), _TargetTypeImages);

                tmpTargetType[4] = this.getMaxSimilarity(this.getPieces(pnTarget2.PointToScreen(pnTargetSupplyType2_2.Location).X
                    , pnTarget2.PointToScreen(pnTargetSupplyType2_2.Location).Y
                    , _Target_SupplyType.Width
                    , _Target_SupplyType.Height), _TargetTypeImages);

                tmpTargetType[5] = this.getMaxSimilarity(this.getPieces(pnTarget2.PointToScreen(pnTargetSupplyType2_3.Location).X
                    , pnTarget2.PointToScreen(pnTargetSupplyType2_3.Location).Y
                    , _Target_SupplyType.Width
                    , _Target_SupplyType.Height), _TargetTypeImages);

                tmpTargetType[6] = this.getMaxSimilarity(this.getPieces(pnTarget3.PointToScreen(pnTargetSupplyType3_1.Location).X
                    , pnTarget3.PointToScreen(pnTargetSupplyType3_1.Location).Y
                    , _Target_SupplyType.Width
                    , _Target_SupplyType.Height), _TargetTypeImages);

                tmpTargetType[7] = this.getMaxSimilarity(this.getPieces(pnTarget3.PointToScreen(pnTargetSupplyType3_2.Location).X
                    , pnTarget3.PointToScreen(pnTargetSupplyType3_2.Location).Y
                    , _Target_SupplyType.Width
                    , _Target_SupplyType.Height), _TargetTypeImages);

                tmpTargetType[8] = this.getMaxSimilarity(this.getPieces(pnTarget3.PointToScreen(pnTargetSupplyType3_3.Location).X
                    , pnTarget3.PointToScreen(pnTargetSupplyType3_3.Location).Y
                    , _Target_SupplyType.Width
                    , _Target_SupplyType.Height), _TargetTypeImages);
                #endregion

                #region TargetSize 1-9
                int[] tmpTargetSize = new int[9];

                tmpTargetSize[0] = this.getMaxSimilarity(this.getPieces(pnTarget1.PointToScreen(pnTargetSupplyNumber1_1.Location).X
                    , pnTarget1.PointToScreen(pnTargetSupplyNumber1_1.Location).Y
                    , _Target_SupplyNumber.Width
                    , _Target_SupplyNumber.Height), _TargetSizeImages);

                tmpTargetSize[1] = this.getMaxSimilarity(this.getPieces(pnTarget1.PointToScreen(pnTargetSupplyNumber1_2.Location).X
                    , pnTarget1.PointToScreen(pnTargetSupplyNumber1_2.Location).Y
                    , _Target_SupplyNumber.Width
                    , _Target_SupplyNumber.Height), _TargetSizeImages);

                tmpTargetSize[2] = this.getMaxSimilarity(this.getPieces(pnTarget1.PointToScreen(pnTargetSupplyNumber1_3.Location).X
                    , pnTarget1.PointToScreen(pnTargetSupplyNumber1_3.Location).Y
                    , _Target_SupplyNumber.Width
                    , _Target_SupplyNumber.Height), _TargetSizeImages);

                tmpTargetSize[3] = this.getMaxSimilarity(this.getPieces(pnTarget2.PointToScreen(pnTargetSupplyNumber2_1.Location).X
                    , pnTarget2.PointToScreen(pnTargetSupplyNumber2_1.Location).Y
                    , _Target_SupplyNumber.Width
                    , _Target_SupplyNumber.Height), _TargetSizeImages);

                tmpTargetSize[4] = this.getMaxSimilarity(this.getPieces(pnTarget2.PointToScreen(pnTargetSupplyNumber2_2.Location).X
                    , pnTarget2.PointToScreen(pnTargetSupplyNumber2_2.Location).Y
                    , _Target_SupplyNumber.Width
                    , _Target_SupplyNumber.Height), _TargetSizeImages);

                tmpTargetSize[5] = this.getMaxSimilarity(this.getPieces(pnTarget2.PointToScreen(pnTargetSupplyNumber2_3.Location).X
                    , pnTarget2.PointToScreen(pnTargetSupplyNumber2_3.Location).Y
                    , _Target_SupplyNumber.Width
                    , _Target_SupplyNumber.Height), _TargetSizeImages);

                tmpTargetSize[6] = this.getMaxSimilarity(this.getPieces(pnTarget3.PointToScreen(pnTargetSupplyNumber3_1.Location).X
                    , pnTarget3.PointToScreen(pnTargetSupplyNumber3_1.Location).Y
                    , _Target_SupplyNumber.Width
                    , _Target_SupplyNumber.Height), _TargetSizeImages);

                tmpTargetSize[7] = this.getMaxSimilarity(this.getPieces(pnTarget3.PointToScreen(pnTargetSupplyNumber3_2.Location).X
                    , pnTarget3.PointToScreen(pnTargetSupplyNumber3_2.Location).Y
                    , _Target_SupplyNumber.Width
                    , _Target_SupplyNumber.Height), _TargetSizeImages);

                tmpTargetSize[8] = this.getMaxSimilarity(this.getPieces(pnTarget3.PointToScreen(pnTargetSupplyNumber3_3.Location).X
                    , pnTarget3.PointToScreen(pnTargetSupplyNumber3_3.Location).Y
                    , _Target_SupplyNumber.Width
                    , _Target_SupplyNumber.Height), _TargetSizeImages);

                #endregion

                #region Fix 3 Spoons
                int[] tmpSpoonSize = new int[3];

                tmpSpoonSize[0] = this.getMaxSimilarity(this.getPieces(pnSpoon1.PointToScreen(pnSpoonSize1.Location).X
                    , pnSpoon1.PointToScreen(pnSpoonSize1.Location).Y
                    , _SpoonSize_Size.Width
                    , _SpoonSize_Size.Height), _SpoonSizeImages);


                tmpSpoonSize[1] = this.getMaxSimilarity(this.getPieces(pnSpoon2.PointToScreen(pnSpoonSize2.Location).X
                    , pnSpoon2.PointToScreen(pnSpoonSize2.Location).Y
                    , _SpoonSize_Size.Width
                    , _SpoonSize_Size.Height), _SpoonSizeImages);

                tmpSpoonSize[2] = this.getMaxSimilarity(this.getPieces(pnSpoon3.PointToScreen(pnSpoonSize3.Location).X
                    , pnSpoon3.PointToScreen(pnSpoonSize3.Location).Y
                    , _SpoonSize_Size.Width
                    , _SpoonSize_Size.Height), _SpoonSizeImages);
                #endregion


                #region Prepare array for solver

                for (int i = 0; i < tmpTargetSize.Length; i++)
                {
                    if (tmpTargetType[i] == 0)//A
                    {
                        if(i<3) first[0] = tmpTargetSize[i] + 1;
                        else if (i<6) second[0] = tmpTargetSize[i] + 1;
                        else third[0] = tmpTargetSize[i] + 1;
                    }
                    else if (tmpTargetType[i] == 1)//B
                    {
                        if(i<3) first[1] = tmpTargetSize[i] + 1;
                        else if (i<6) second[1] = tmpTargetSize[i] + 1;
                        else third[1] = tmpTargetSize[i] + 1;
                    }
                    else if (tmpTargetType[i] == 2)//C
                    {
                        if(i<3) first[2] = tmpTargetSize[i] +1;
                        else if (i<6) second[2] = tmpTargetSize[i]+1;
                        else third[2] = tmpTargetSize[i]+1;
                    }
                    else if (tmpTargetType[i] == 3)//ว่าง
                    {
                        
                    }
                }

                //SpoonSize
                for (int i = 0; i < tmpSpoonSize.Length; i++)
                {
                    spoons[i] = tmpSpoonSize[i] + 2;
                }

                #endregion  


                //Print Detect result
                string result = string.Empty;
                for (int i = 0; i < first.Length; i++)
                {
                    result += first[i].ToString() + ", ";
                }

                result += " ---- " + spoons[0];
                result += Environment.NewLine;

                for (int i = 0; i < second.Length; i++)
                {
                    result += second[i].ToString() + ", ";
                }

                result += " ---- " + spoons[1];
                result += Environment.NewLine;

                for (int i = 0; i < third.Length; i++)
                {
                    result += third[i].ToString() + ", ";
                }

                result += " ---- " + spoons[2];
                result += Environment.NewLine;

                Console.WriteLine(result);
            }


            // set point
            Panel[] dishPanels = new Panel[] { pnTarget1, pnTarget2, pnTarget3 };
            Panel[] spoonPanels = new Panel[] { pnSpoon1, pnSpoon2, pnSpoon3 };
            Panel[] foodPanels = new Panel[] { pnSupply1, pnSupply2, pnSupply3 };

            _ClickDishPoints = new Point[3];
            _ClickSpoonPoints = new Point[3];
            _ClickFoodPoints = new Point[3];

            for (int i = 0; i < 3; i++)
            {
                _ClickDishPoints[i] = new Point(panel2.PointToScreen(dishPanels[i].Location).X + dishPanels[i].Width / 2,
                                             panel2.PointToScreen(dishPanels[i].Location).Y + dishPanels[i].Height / 2);

                _ClickSpoonPoints[i] = new Point(panel2.PointToScreen(spoonPanels[i].Location).X + spoonPanels[i].Width / 2,
                                             panel2.PointToScreen(spoonPanels[i].Location).Y + spoonPanels[i].Height / 2);

                _ClickFoodPoints[i] = new Point(panel2.PointToScreen(foodPanels[i].Location).X + foodPanels[i].Width / 2,
                                             panel2.PointToScreen(foodPanels[i].Location).Y + foodPanels[i].Height / 2);

                //Console.WriteLine(_ClickDishPoints[i].ToString() + " " + _ClickSpoonPoints[i].ToString() + " " + _ClickFoodPoints[i].ToString());
            }


            int countStep = 0;
            steps = FoodSolverNameSpace.FoodSolver.Solve(first, second, third, spoons, ref countStep);

            this.Hide();

            // clicking
            isRunning = true;
            new Thread(new ThreadStart(Clicking)).Start();
        }

        Point[] _ClickDishPoints;
        Point[] _ClickSpoonPoints;
        Point[] _ClickFoodPoints;

        private delegate void d_Show();
        private bool isRunning = false;
        public void Clicking()
        {
            int focusSpoon = -1;

            for (int i = 0; i < steps.Length; i++)
            {
                if (isRunning == false) break;

                char[] c = steps[i].ToCharArray();

                int numSpoon = int.Parse(c[0]+"");

                if (numSpoon != focusSpoon)
                {
                    // unfocus old spoon
                    if (focusSpoon != -1)
                    {
                        csMouseClick.MoveCursorTo(_ClickSpoonPoints[focusSpoon].X, _ClickSpoonPoints[focusSpoon].Y, 0, 0);
                        csMouseClick.LeftClick(_DelayMs);
                        Thread.Sleep(_DelayMs);
                    }

                    // focus new spoon
                    csMouseClick.MoveCursorTo(_ClickSpoonPoints[numSpoon].X, _ClickSpoonPoints[numSpoon].Y, 0, 0);
                    csMouseClick.LeftClick(_DelayMs);
                    Thread.Sleep(_DelayMs);

                    focusSpoon = numSpoon;
                }

                int target = int.Parse(c[2] + "");
                int x = 0;
                int y = 0;

                // target another spoon
                if (c[1] == 'P')
                {
                    x = _ClickSpoonPoints[target].X;
                    y = _ClickSpoonPoints[target].Y;
                }
                // target food
                else if (c[1] == 'F')
                {
                    x = _ClickFoodPoints[target].X;
                    y = _ClickFoodPoints[target].Y;
                }
                // target dish
                else if (c[1] == 'S')
                {
                    x = _ClickDishPoints[target].X;
                    y = _ClickDishPoints[target].Y;
                }

                csMouseClick.MoveCursorTo(x, y, 0, 0);
                csMouseClick.LeftClick(_DelayMs);
                Thread.Sleep(_DelayMs);

                //force last click
                if (i == steps.Length - 1 && _IsForceDoubleClick)
                {
                    csMouseClick.MoveCursorTo(x, y, 0, 0);
                    csMouseClick.LeftClick(_DelayMs);
                    Thread.Sleep(_DelayMs);
                }
            }

            this.Invoke(new d_Show(Show));
        }

        private Bitmap getPieces(int startX, int startY, int Width, int Height)
        {
            return screenshot.Clone(new Rectangle(startX, startY, Width, Height), screenshot.PixelFormat);
        }

        ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0);
        private int getMaxSimilarity(Bitmap ImageToCompare, Bitmap[] Template)
        {
            Double[] score = new double[Template.Length];

            double max = Double.MinValue;
            for (int i = 0; i < Template.Length; i++)
            {
                TemplateMatch[] matchings = tm.ProcessImage(ImageToCompare, Template[i]);
                score[i] = matchings[0].Similarity;

                if (max < score[i])
                {
                    max = score[i];
                }
            }

            return Array.IndexOf(score, max);
        }

        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString().ToUpper() == "SPACE")
            {
                isRunning = false;
            }
        }

        private void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            //textBoxLog.AppendText(string.Format("MouseDown - {0}\n", e.Button));
            //textBoxLog.ScrollToCaret();

            if (e.Button.ToString() == "Right")
            {
                isRunning = false;
            }   
        }
    }
}
