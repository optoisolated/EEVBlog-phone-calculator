using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EEVblog_Phone_Calulator
{
    public partial class MainPage : ContentPage
	{
        static Label MakeLabel()
        {
            var output = new Label();
            output.VerticalTextAlignment = TextAlignment.Center;
            output.HorizontalTextAlignment = TextAlignment.Center;
            output.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
            output.VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
            output.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            return output;
        }

        Button Plus, Minus, Divide, Multiply;
        Button[] Keypad;

        Label ResultLabel = MakeLabel();
        Label BufferLabel = MakeLabel();

        private double _Buffer = 0;
        public double Buffer
        {
            get
            {
                return _Buffer;
            }
            set
            {
                _Buffer = value;
                BufferLabel.Text = _Buffer.ToString();
            }
        }

        private double _Result;
        public double Result
        {
            get
            {
                return _Result;
            }
            set
            {
                _Result = value;
                ResultLabel.Text = _Result.ToString();
            }
        }

		public MainPage()
		{
			InitializeComponent();

            //Operation buttons
            var button_font     =   Device.GetNamedSize(NamedSize.Large, typeof(Button));
            var button_layout   =   new LayoutOptions(LayoutAlignment.Center, false);
            Plus                =   new Button() { Text = "+" ,    FontSize = button_font, BackgroundColor = Color.Gray};
            Minus               =   new Button() { Text = "-",    FontSize = button_font, BackgroundColor = Color.Gray};
            Divide              =   new Button() { Text = "/",   FontSize = button_font, BackgroundColor = Color.Gray};
            Multiply            =   new Button() { Text = "x", FontSize = button_font, BackgroundColor = Color.Gray};
            Plus.Pressed        +=  Plus_Pressed;
            Minus.Pressed       +=  Minus_Pressed;
            Divide.Pressed      +=  Divide_Pressed;
            Multiply.Pressed    +=  Multiply_Pressed;

            //Keypad buttons
            Keypad = new Button[10];
            for(var index = 0; index < Keypad.Count(); ++index)
            {
                ref var button = ref Keypad[index];
                button = new Button() { Text = index.ToString(), FontSize = button_font };
                int i = index;
                button.Pressed += (o, e) => { KeypadButton_Pressed(i); };
            }

            //Setup the grid
            int[] index_remapper = { 0, 3, 2, 1, 6, 5, 4, 9, 8, 7 };
            var grid = new rMultiplatform.AutoGrid();

            //Define the calculate grid
            grid.DefineGrid(3, 6);
            grid.AutoAdd(BufferLabel);
            grid.AutoAdd(ResultLabel, 2);

            //Set the keypad grid positions
            for (var index = Keypad.Count() - 1; index >= 0 ; --index)
            {
                ref var button = ref Keypad[index_remapper[index]];
                if (index != 0)     grid.AutoAdd(button);
                else                grid.AutoAdd(button, 2);
            }

            //Setup the buttons
            grid.AutoAdd(Plus);
            grid.AutoAdd(Minus);
            grid.AutoAdd(Divide);
            grid.AutoAdd(Multiply);

            //Show it hello
            Content = grid;
        }

        #region Operation_Buttons
        private void KeypadButton_Pressed(int index)
        {
            Buffer *= 10.0;
            Buffer += (double)index;
        }
        #endregion
        #region Operation_Buttons
        /////////////////////////////////////////////////////////////
        //Operation buttons
        private void Multiply_Pressed(object sender, EventArgs e)
        {
            Result *= Buffer;
            Buffer = 0.0;
        }
        private void Divide_Pressed(object sender, EventArgs e)
        {
            if (Buffer == 0)
            {
                BufferLabel.Text = "Cannot divide by zero.";
            }
            else
            {
                Result /= Buffer;
                Buffer = 0.0;
            }
        }
        private void Minus_Pressed(object sender, EventArgs e)
        {
            Result -= Buffer;
            Buffer = 0.0;
        }
        private void Plus_Pressed(object sender, EventArgs e)
        {
            Result += Buffer;
            Buffer = 0.0;
        }
        /////////////////////////////////////////////////////////////
#endregion
    }
}