using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private List<string> calculationHistory = new List<string>();
        public Form1()
        {
            InitializeComponent();
            // 배용진
            Calculation("10 + (20 - 10) * 30 / 2");     //테스트용 생성자 추후 삭제
            Split_String("받아올 문자열 혹은 함수");      //테스트용 생성자 추후 삭제
            // 배용진 여기까지
        }

        // 배용진
        // 문자열을 받아와서 스플릿한 객체를 공백을 체크해서 배열 형태로 내보냄.
        // 필요가...없어졌나?
        public string[] Split_String(string s_calc)
        {
            string[] s_array = s_calc.Split(' '); ;

            return s_array;
        }

        // 문자열 내용을 받아와서 사칙연산 후 string으로 반환
        // split을 써야하나?? -> 문자열을 받아서 가능하게 바꿈 -> 이러면 split 기능이 쓸모 없어지지 않았나..?
        // 대신 공학용 계산기처럼 괄호 넣어도 계산이 됨.
        public string Calculation(string input)
        {
            // 데이터테이블 객체 생성
            DataTable dt = new DataTable();
            // 정확한 계산을 위해 decimal로 선언
            decimal result = 0;

            // string으로 받은 문자열을 compute()로 계산. 공백 구분 없이 사칙연산 해줌..
            // 이후 decimal로 변환한 값을 result에 저장
            result = Convert.ToDecimal(dt.Compute(input, ""));
            // 배용진 여기까지

            // 김정관 시작
            // 계산식과 그 결과를 이력에 추가합니다.
            string currentCalculation = input;
            string currentResult = result.ToString();
            calculationHistory.Add(currentCalculation + " = " + currentResult);
            // 현재 계산식(input)과 결과(result)를 문자열 형태로 조합하여 계산 이력 (calculationHistory) 리스트에 추가

            // 계산 이력의 크기를 제한합니다. 여기서는 최근 10개 항목까지 유지
            const int maxHistorySize = 10;
            if (calculationHistory.Count > maxHistorySize)
            {
                calculationHistory.RemoveAt(0);//'0'번째 인덱스 삭제(첫번째 요소)
            }
            // 계산 이력 리스트의 크기가 최대 크기(maxHistorySize)를 초과하면, 가장 오래된 항목을 제거하여 리스트 크기를 10개로 유지

            // 현재 결과 반환
            return currentResult;
            // 김정관 끝
        }

        // 이남웅
        // textBox의 입력 및 필터링 함수
        private string allowedChars = "0123456789.+-*/%()";

        private void textBox_input_KeyPress(object sender, KeyPressEventArgs e)
        {
            char key = e.KeyChar;

            // 입력된 문자가 허용된 문자 목록에 속하지 않으면 입력을 취소
            if (allowedChars.IndexOf(key) == -1)
            {
                e.Handled = true;
            }
        }

        // 이남웅
        // textBox의 입력을 실시간으로 처리하는 함수
        private void textBox_input_TextChanged(object sender, EventArgs e)
        {
            // TextBox 내용을 전부 검사하여 잘못된 문자를 제거
            string inputText = textBox_input.Text;
            string filteredText = new string(inputText.Where(c => allowedChars.Contains(c)).ToArray());

            textBox_print.Text = filteredText;
            textBox_input.Text = filteredText;

            // 커서 위치를 TextBox 끝으로 이동
            textBox_input.SelectionStart = textBox_input.Text.Length;
        }

        // 이남웅
        // C 버튼 함수
        private void button_res_Click(object sender, EventArgs e)
        {
            // 입력된 문자 초기화
            textBox_input.Text = "";
        }

        // 이남웅
        // DEL 버튼 함수
        private void button_del_Click(object sender, EventArgs e)
        {
            // 만약 입력된 문자가 존재할 경우
            if (!string.IsNullOrEmpty(textBox_input.Text))
            {
                // 마지막 문자를 제거하여 나머지 부분을 대입
                textBox_input.Text = textBox_input.Text.Substring(0, textBox_input.Text.Length - 1);
            }
        }

        // 배용진
        // = 버튼 함수
        private void button_ica_Click(object sender, EventArgs e)
        {
            // 입력 받았을 때 계산 텍스트 변경 및 텍스트박스 리셋
            textBox_print.Text = Calculation(textBox_input.Text);
        }

        //김정관 시작
        private void button_log_Click(object sender, EventArgs e) // 로그버튼 클릭 이벤트의 핸들러메소드
        {
            // 최근 10개의 계산 이력을 가져오는데, 계산 이력이 10개 미만인 경우 전체 이력을 가져옵니다.
            int count = Math.Min(calculationHistory.Count, 10); // 리스트의 항목 수와 10중에서 작은값 표시

            // StringBuilder를 사용하여 이력 텍스트 작성
            StringBuilder historyText = new StringBuilder();
            for (int i = calculationHistory.Count - count; i < calculationHistory.Count; i++)
            // count개 항목에 대해서 반복, 루푸는 count개 항목 중 가장 오래도니 항목의 인덱스에서 시작해서 리스트 마지막까지 반복
            {
                historyText.AppendLine(calculationHistory[i]);
                // 각 계산 이력항목을 historyText에 추가 및 새 줄로 구분
            }

            // 계산 이력을 textBox_print에 표시
            string fullText = "계산 이력 (최근 " + count + "개):\r\n\r\n" + historyText.ToString();
            // textBox_print 컨트롤의 텍스트를 fullText로 설정해 계산이력 서식화 및 표시
            textBox_print.Text = fullText;

            // 필요한 경우, 스크롤을 맨 위로 이동
            //textBox_print.SelectionStart = 100; // 컨트롤 텍스트 시작위치를 0으로 설정
            //textBox_print.ScrollToCaret(); // 컨트롤을 스크롤해 텍스트의 시작 부분에 위치하도록 함 -> 가장 최근계산이력 보이게 하기
        }
        // 김정관 끝
            //textBox_input.Text = "";
        }

        // 배용진
        // 더하기 버튼, 누르면 계산기에 더하기 추가됨
        private void button_plu_Click(object sender, EventArgs e)
        {
            string text = textBox_input.Text;
            char lastcharacter = default;
            if (textBox_input.Text == "")
            {
                lastcharacter = default;
            }
            else
            {
                lastcharacter = text[text.Length - 1];
            }

            // 기호 있으면 교체
            if (lastcharacter.ToString() == "+" || lastcharacter.ToString() == "-" ||
                lastcharacter.ToString() == "*" || lastcharacter.ToString() == "/" ||
                lastcharacter.ToString() == "%")
            {
                textBox_input.Text = text.Remove(text.Length - 1);
                textBox_input.Text += "+";
            }
            else if (lastcharacter == default || lastcharacter.ToString() == "-")
            {
                // 아무것도 입력되지 않는 상태일 때 작동
                textBox_input.Text = "";
            }
            else
            {
                textBox_input.Text += "+";
            }
        }

        // 배용진
        // 빼기 버튼
        private void button_min_Click(object sender, EventArgs e)
        {
            string text = textBox_input.Text;
            char lastcharacter = default;
            if (textBox_input.Text == "")
            {
                lastcharacter = default;
            }
            else
            {
                lastcharacter = text[text.Length - 1];
            }

            // 기호 있으면 교체
            if (lastcharacter.ToString() == "+" || lastcharacter.ToString() == "-" ||
                lastcharacter.ToString() == "*" || lastcharacter.ToString() == "/" ||
                lastcharacter.ToString() == "%")
            {
                textBox_input.Text = text.Remove(text.Length - 1);
                textBox_input.Text += "-";
            }
            else if (lastcharacter == default || lastcharacter.ToString() == "-")
            {
                // 아무것도 입력되지 않는 상태일 때 작동
                textBox_input.Text += "-";
            }
            else
            {
                textBox_input.Text += "-";
            }
        }
        // 배용진
        // 곱하기 버튼
        private void button_mul_Click(object sender, EventArgs e)
        {
            
            string text = textBox_input.Text;
            char lastcharacter = default;
            if (textBox_input.Text == "")
            {
                lastcharacter = default;
            }
            else
            {
                lastcharacter = text[text.Length - 1];
            }

            // 기호 있으면 교체
            if (lastcharacter.ToString() == "+" || lastcharacter.ToString() == "-" ||
                lastcharacter.ToString() == "*" || lastcharacter.ToString() == "/" ||
                lastcharacter.ToString() == "%" )
            {
                textBox_input.Text = text.Remove(text.Length - 1);
                textBox_input.Text += "*";
            }
            else if (lastcharacter == default || lastcharacter.ToString() == "-")
            {
                // 아무것도 입력되지 않는 상태일 때 작동
                textBox_input.Text = "";
            }
            else
            {
                textBox_input.Text += "*";
            }
        }
        // 배용진
        // 나누기 버튼 ( 몫 )
        private void button_div_Click(object sender, EventArgs e)
        {
            string text = textBox_input.Text;
            char lastcharacter = default;
            if (textBox_input.Text == "")
            {
                lastcharacter = default;
            }
            else
            {
                lastcharacter = text[text.Length - 1];
            }

            // 기호 있으면 교체
            if (lastcharacter.ToString() == "+" || lastcharacter.ToString() == "-" ||
                lastcharacter.ToString() == "*" || lastcharacter.ToString() == "/" ||
                lastcharacter.ToString() == "%")
            {
                textBox_input.Text = text.Remove(text.Length - 1);
                textBox_input.Text += "/";
            }
            else if (lastcharacter == default || lastcharacter.ToString() == "-")
            {
                // 아무것도 입력되지 않는 상태일 때 작동
                textBox_input.Text += "";
            }
            else
            {
                textBox_input.Text += "/";
            }
        }
        // 배용진
        // 나누기 버튼 ( 나머지 )
        private void button_rem_Click(object sender, EventArgs e)
        {
            string text = textBox_input.Text;
            char lastcharacter = default;
            if (textBox_input.Text == "")
            {
                lastcharacter = default;
            }
            else
            {
                lastcharacter = text[text.Length - 1];
            }

            // 기호 있으면 교체
            if (lastcharacter.ToString() == "+" || lastcharacter.ToString() == "-" ||
                lastcharacter.ToString() == "*" || lastcharacter.ToString() == "/" ||
                lastcharacter.ToString() == "%" )
            {
                textBox_input.Text = text.Remove(text.Length - 1);
                textBox_input.Text += "%";
            }
            else if (lastcharacter == default || lastcharacter.ToString() == "-")
            {
                // 아무것도 입력되지 않는 상태일 때 작동
                textBox_input.Text = "";
            }
            else
            {
                textBox_input.Text += "%";
            }
        }

        // 배용진 여기까지

        // 배용진
        // 
        }

        // 배용진 여기까지

        // 배용진
        // 
    }
}