using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Parser
{
    public static List<List<string>> ContractParse(TextAsset data)
    {
        List<List<string>> returnList = new List<List<string>>();
        StringReader sr = new StringReader(data.text);

        sr.ReadLine();                  // 제일 윗 줄을 건너뛴다.
        string source = sr.ReadLine();  // 먼저 한줄을 읽는다.
        string[] values;                // 구분된 데이터들을 저장할 배열 (values[0]이면 첫번째 데이터 )

        while (source != null)          // 비어있을 때까지 읽는다
        {
            values = source.Split('\t');                // tab으로 구분한다
            List<string> dialog = new List<string>();   // 하나의 계약 내용을 저장하는 리스트

            for (int i = 0; i < values.Length; i++)
            {
                dialog.Add(values[i]);
            }

            returnList.Add(dialog);     // 저장

            source = sr.ReadLine();     // 한줄 읽는다.            
        }

        return returnList;
    }

    public static List<string> SpeechParse(TextAsset data)
    {
        List<string> returnList = new List<string>();
        StringReader sr = new StringReader(data.text);

        sr.ReadLine();                  // 제일 윗 줄을 건너뛴다.
        string source = sr.ReadLine();  // 먼저 한줄을 읽는다.

        while (source != null)          // 비어있을 때까지 읽는다
        {
            returnList.Add(source);     // 저장

            source = sr.ReadLine();     // 한줄 읽는다.            
        }

        return returnList;
    }
}