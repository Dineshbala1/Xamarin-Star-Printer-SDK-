using Android.Widget;
using Android.OS;
using static Com.Starmicronics.Starioextension.StarIoExt;
using System.Collections.Generic;
using Com.Starmicronics.Stario;
using Com.Starmicronics.Starioextension;
using System.Threading.Tasks;
using System.Text;
using System;
using Android.Graphics;
using Android.Views;
using Android.Graphics.Drawables;
using Android.Content;
using Android.Text;
using Android.App;

namespace test
{
    [Activity(Label = "test", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity, IDialogInterfaceOnDismissListener
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);
            button.Text = "Click to start printing operation";

            IList<PortInfo> mPortList = new List<PortInfo>();

            //Search the port.
            await Task.Factory.StartNew(() =>
            {
                //This should give the list of available bluetooth printers
                mPortList = StarIOPort.SearchPrinter("TCP:");
            });

            button.Click += delegate
            {
                //if (mPortList.Count > 0)
                //{

                //    await Task.Factory.StartNew(() =>
                //     {
                //         try
                //         {
                //             // Printer name can be changed -> Bluetooth Printer BT:MacAddress or BT:PrinterName
                //             var port = StarIOPort.GetPort("TCP:192.168.1.18", "", 10000, this);
                //             var status = port.BeginCheckedBlock();

                //             // Harcoded for test purpose to evaluate the StarIOlibrary
                //             // StarPRNT - portable printer so this has been selected - 
                //             // Need more information if this fails to understand the Star Manual
                //             ICommandBuilder builder = CreateCommandBuilder(Emulation.StarGraphic);
                //             builder.BeginDocument();
                //             builder.AppendCodePage(CommandBuilderCodePageType.Utf8);
                //             builder.AppendInternational(CommandBuilderInternationalType.Usa);
                //             builder.AppendCharacterSpace(0);
                //             builder.AppendAlignment(CommandBuilderAlignmentPosition.Center);
                //             builder.AppendBitmap(GetBitmapFromtext(), true);
                //             builder.AppendCutPaper(CommandBuilderCutPaperAction.PartialCutWithFeed);
                //             builder.EndDocument();
                //             var result = builder.GetCommands();

                //             port.WritePort(result, 0, result.Length);

                //             status = port.EndCheckedBlock();
                //             if (status.Offline == false)
                //             {
                //                 // This means the printing has been successul
                //                 this.RunOnUiThread(() =>
                //                        {
                //                            Toast.MakeText(this, "Success", ToastLength.Long).Show();
                //                        });
                //             }
                //             else
                //             {
                //                 //Failure Case

                //             }
                //         }
                //         catch (Exception ex)
                //         {
                //             System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                //         }
                //     });
                //}
                //else
                //{
                //    Toast.MakeText(this, "No Printer Found", ToastLength.Long).Show();
                //}

                if (!Com.Starmicronics.Cloudservices.CloudServices.IsRegistered(this))
                {
                    Com.Starmicronics.Cloudservices.CloudServices.ShowRegistrationView(this.FragmentManager, this);
                }
            };
        }

        public static Bitmap getBitmapFromView(View view)
        {
            //Define a bitmap with the same size as the view
            view.DrawingCacheEnabled = true;
            view.BuildDrawingCache();
            return view.GetDrawingCache(true);
        }

        public Bitmap GetBitmapFromtext()
        {
            //Convert the sample text into Bitmap for printing.
            Paint paint = new Paint();
            Bitmap bitmap;
            Canvas canvas;

            paint.TextSize = (22);
            string printText = "Test Print Data to evaluate the starIO printer";
            paint.GetTextBounds(printText, 0, printText.Length, new Rect());

            TextPaint textPaint = new TextPaint(paint);
            StaticLayout staticLayout = new StaticLayout(printText, textPaint, 576, Layout.Alignment.AlignNormal, 1, 0, false);

            // Create bitmap
            bitmap = Bitmap.CreateBitmap(staticLayout.Width, staticLayout.Height, Bitmap.Config.Argb8888);

            // Create canvas
            canvas = new Canvas(bitmap);
            canvas.DrawColor(Color.White);
            canvas.Translate(0, 0);
            staticLayout.Draw(canvas);
            return bitmap;
        }

        public void OnDismiss(IDialogInterface dialog)
        {
            
        }
    }

    //Tryout code for drawing the text as bitmap

    //LinearLayout linearLayout = FindViewById<LinearLayout>(Resource.Id.myLayout);
    //var exLayout = new ExtLinearLayout(this);
    //exLayout.LayoutParameters = new ViewGroup.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
    //exLayout.SetBackgroundColor(Android.Graphics.Color.Red);
    //linearLayout.AddView(exLayout);

    //var bitmap = GetBitmapFromtext();
    //exLayout.BitmapProperty = bitmap;
    //exLayout.Invalidate();
    //return;

    //internal class ExtLinearLayout : LinearLayout
    //{
    //    public Bitmap BitmapProperty
    //    {
    //        get;
    //        set;
    //    }

    //    Paint paint = new Paint();

    //    public ExtLinearLayout(Context mainActivity) : base(mainActivity)
    //    {
    //        SetWillNotDraw(false);
    //    }

    //    protected override void OnDraw(Canvas canvas)
    //    {
    //        base.OnDraw(canvas);
    //        if (BitmapProperty == null)
    //            return;
    //        canvas.DrawBitmap(BitmapProperty, 10, 10, paint);
    //    }
    //}
}


