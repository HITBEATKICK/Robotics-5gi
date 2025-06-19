using ActUtlType64Lib;

namespace MxComp
{
    public partial class Form1 : Form
    {
        ActUtlType64 mxComponent;
        bool isConnected;

        public Form1()
        {
            InitializeComponent();

            label1.Text = "���α׷��� �����մϴ�.";

            mxComponent = new ActUtlType64();

            mxComponent.ActLogicalStationNumber = 1;
        }

        private void Exit(object sender, FormClosingEventArgs e)
        {
            Close(sender, e);
        }

        private void Open(object sender, EventArgs e)
        {
            int iRet = mxComponent.Open();

            if (iRet == 0)
            {
                isConnected = true;

                label1.Text = "�� ������ �Ǿ����ϴ�.";
            }
            else
            {
                // �����ڵ� ��ȯ(16����)
                label1.Text = Convert.ToString(iRet, 16);
            }
        }

        private void Close(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                label1.Text = "�̹� �������� �����Դϴ�.";

                return;
            }

            int iRet = mxComponent.Close();

            if (iRet == 0)
            {
                isConnected = false;

                label1.Text = "�� ������ �Ǿ����ϴ�.";
            }
            else
            {
                // �����ڵ� ��ȯ(16����)
                label1.Text = Convert.ToString(iRet, 16);
            }
        }

        private void GetDevice(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                label1.Text = "����̽� �̸��� �Է��� �ּ���.";
                return;
            }

            int data = 0;
            int iRet = mxComponent.GetDevice(textBox1.Text, out data);

            if (iRet == 0)
            {
                label1.Text = $"{textBox1.Text}: {data}";
            }
            else
            {
                // �����ڵ� ��ȯ(16����)
                label1.Text = Convert.ToString(iRet, 16);
            }
        }

        private void SetDevice(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty)
            {
                label1.Text = "����̽� �̸� �Ǵ� ���� �Է��� �ּ���.";
                return;
            }

            int value = 0;
            bool isOk = int.TryParse(textBox2.Text, out value);

            if (!isOk)
            {
                label1.Text = "���ڸ� �Է��� �ּ���.";
                return;
            }

            int iRet = mxComponent.SetDevice(textBox1.Text, value);

            if (iRet == 0)
            {
                label1.Text = $"{textBox1.Text}: {textBox2.Text}�� �� ����Ǿ����ϴ�.";
            }
            else
            {
                // �����ڵ� ��ȯ(16����)
                label1.Text = Convert.ToString(iRet, 16);
            }
        }

        private void ReadDeviceBlock(object sender, EventArgs e)
        {
            int deviceBlockCnt = 0;
            bool isOk = int.TryParse(textBox3.Text, out deviceBlockCnt);

            if (!isOk)
            {
                label1.Text = "����� ������ ���������� �Է��� �ּ���.";

                return;
            }

            int[] data = new int[deviceBlockCnt];
            int iRet = mxComponent.ReadDeviceBlock(textBox1.Text, deviceBlockCnt, out data[0]);

            if (iRet == 0)
            {
                // 3�� ��� -> { 555, 125, 0 }
                // n�� ��� -> { x, y, z, i, j, k, }
                // X0���� �� ��� ���� result[0] = X0
                string result = "{ ";
                for (int i = 0; i < data.Length; i++)
                {
                    // 1. 10���� 336 -> 2���� 101010000
                    string binary = Convert.ToString(data[i], 2);

                    // 2. ���ư� ������Ʈ�� �߰����ش�. 1/0101/0000 -> 0000/0001/0101/0000
                    int strLength = binary.Length; // 9
                    int upBitCnt = 16 - strLength; // 16 - 9 = 7

                    // 3. ������ 1/0101/0000 -> 0000/1010/1
                    string reversedBinary = new string(binary.Reverse().ToArray());

                    // 4. ������Ʈ ���̱� 0000/1010/1 + 000/0000
                    for (int j = 0; j < upBitCnt; j++)
                    {
                        reversedBinary += "0";
                    }

                    result += reversedBinary + ", "; // 0000/1010/1000/0000
                }
                result += " }";

                label1.Text = result; // { 0000101010000000, 0000101010000000 }
            }
        }

        // �ּ�: ����� ù ����Ʈ ���� 
        private void WriteDeviceBlock(object sender, EventArgs e)
        {
            // ��� ����: 3, ����̽� ��: "333,55,2"
            int deviceBlockCnt = 0;
            bool isOk = int.TryParse(textBox3.Text, out deviceBlockCnt);

            if (!isOk)
            {
                label1.Text = "����� ������ ���������� �Է��� �ּ���.";
                return;
            }

            // ����̽� ��: "333,55,2" -> { 333, 55, 2 }
            string[] values = textBox2.Text.Split(",");
            int[] numbers = Array.ConvertAll(values, int.Parse); // Try.Parse�� ���濹��

            int iRet = mxComponent.WriteDeviceBlock(textBox1.Text, deviceBlockCnt, ref numbers[0]);

            if (iRet == 0)
            {
                label1.Text = "���Ⱑ �Ϸ�Ǿ����ϴ�.";
            }
            else
            {
                label1.Text = Convert.ToString(iRet, 16);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
