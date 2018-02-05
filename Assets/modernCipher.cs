using UnityEngine;
using KMHelper;
using System.Linq;
using System.Collections.Generic;

public class modernCipher : MonoBehaviour {

    public KMAudio Audio;
    public KMBombModule Module;
    public KMBombInfo Info;
    public KMSelectable[] btn;
    public KMSelectable submit, erase;
    public MeshFilter[] wordsCounter;
    public TextMesh Screen, UserScreen;
    public Material ledsMat;

    private static int _moduleIdCounter = 1;
    private int _moduleId = 0;

    private string[] words =
    {
        "SALLY", "BUTTON", "PHONE", "YEAH", "MOTHER", "DADDY", "COMPUTER", "CLOCK", "SCREEN", "RULER",
        "HOUSE", "WEDDING", "ROOM", "SALES", "PIZZA", "LOVE", "CHICKEN", "HORSE", "DANCER", "BOMB",
        "WINDOW", "CONTROL", "CONDOM", "MODULE", "PAPER", "PENCIL", "SCHOOL", "SOCCER", "BASKET", "MOVIES",
        "ITALY", "ENGLAND", "BANANA", "BRAZIL", "PARK", "ZOMBIE", "HORROR", "COMICS", "BOOKS", "DEGREE",
        "FRANCE", "SPAIN", "MOBILE", "SHIP", "SCREEN", "EMAIL", "PLANE", "CABLES", "MANUAL", "PRINTER"
    };
    private Dictionary<string, string> chosenWords;
    private bool _isSolved = false, _lightsOn = false;
    private string ans, encrypted;
    private int stageAmt = 3, stageCur = 1;

    // Use this for initialization
    void Start () {
        _moduleId = _moduleIdCounter++;
        Module.OnActivate += Activate;
    }

    private void Awake()
    {
        submit.OnInteract += delegate ()
        {
            ansChk();
            return false;
        };
        erase.OnInteract += delegate ()
        {
            UserScreen.text = "";
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, erase.transform);
            erase.AddInteractionPunch();
            return false;
        };
        for (int i = 0; i < 26; i++)
        {
            int j = i;
            btn[i].OnInteract += delegate ()
            {
                handlePress(j);
                return false;
            };
        }
    }

    private string intToChar(int i)
    {
        switch (i)
        {
            case 0: return "Q";
            case 1: return "W";
            case 2: return "E";
            case 3: return "R";
            case 4: return "T";
            case 5: return "Y";
            case 6: return "U";
            case 7: return "I";
            case 8: return "O";
            case 9: return "P";
            case 10: return "A";
            case 11: return "S";
            case 12: return "D";
            case 13: return "F";
            case 14: return "G";
            case 15: return "H";
            case 16: return "J";
            case 17: return "K";
            case 18: return "L";
            case 19: return "Z";
            case 20: return "X";
            case 21: return "C";
            case 22: return "V";
            case 23: return "B";
            case 24: return "N";
            case 25: return "M";
        }
        return "";
    }

    void Activate()
    {
        Init();
        _lightsOn = true;
    }

    void Init()
    {
        UserScreen.text = "";
        chosenWords = new Dictionary<string, string>();
        wordsCounter[1].GetComponent<Renderer>().material = ledsMat;
        wordsCounter[2].GetComponent<Renderer>().material = ledsMat;
        wordsCounter[3].GetComponent<Renderer>().material = ledsMat;
        wordsCounter[4].GetComponent<Renderer>().material = ledsMat;
        wordsCounter[0].GetComponent<Renderer>().material = ledsMat;
        stageCur = 1;
        generateStage(1);
    }

    private void generateStage(int num)
    {
        Debug.LogFormat("[Caesar Cipher #{0}] <Stage {1}> START", _moduleId, num);

        do
            ans = words[Random.Range(0, 50)];
        while (chosenWords.Values.Contains(ans));
        chosenWords.Add("Stage"+stageCur, ans);
        encrypted = "";
        Debug.LogFormat("[Caesar Cipher #{0}] <Stage {1}> Picked word from array is {2}", _moduleId, num, ans);

        int key = 0;
        string numbers = "";

        foreach (int number in Info.GetSerialNumberNumbers())
        {
            numbers += number.ToString() + "+";
            key += number;
        }

        key += Info.GetStrikes();

        Debug.LogFormat("[Caesar Cipher #{0}] <Stage {1}> Numbers in serial: {2} = {3} <= this is the key", _moduleId, num, numbers, key);

        if (Info.GetSerialNumberLetters().Any("AEIOU".Contains))
        {
            foreach (char c in ans)
            {
                int position = getPositionFromChar(c);
                position += key;
                if (position > 25)
                {
                    int tmp = -1;
                    while (position > 25)
                    {
                        position--;
                        tmp++;
                    }
                    encrypted += intToChar(tmp);
                }
                else
                    encrypted += intToChar(position);
            }
            Debug.LogFormat("[Caesar Cipher #{0}] <Stage {1}> Serial number contains at least a vowel. Encrypted word is {2}", _moduleId, num, encrypted);
        }
        else if (Info.GetBatteryCount() > 3)
        {
            foreach (char c in ans)
            {
                int position = getPositionFromChar(c);
                position -= key;
                if (position < 0)
                {
                    int tmp = 26;
                    while (position < 0)
                    {
                        position++;
                        tmp--;
                    }
                    encrypted += intToChar(tmp);
                }
                else
                    encrypted += intToChar(position);
            }
            Debug.LogFormat("[Caesar Cipher #{0}] <Stage {1}> Serial number does not contain vowels. More than 3 batteries on the bomb. Encrypted word is {2}", _moduleId, num, encrypted);
        }
        else if (Info.IsPortPresent(KMBombInfoExtensions.KnownPortType.Serial))
        {
            foreach (char c in ans)
            {
                int position = getPositionFromChar(c);
                Debug.LogFormat("[Caesar Cipher #{0}] <Stage {1}> position is {2}", _moduleId, num, position);
                if (stageCur != 1)
                {
                    position += key + chosenWords["Stage" + (stageCur - 1).ToString()].Count(); //sommo alla chiave il numero di lettere della parola precedente
                    if (position > 25)
                    {
                        int tmp = -1;
                        while (position > 25)
                        {
                            position--;
                            tmp++;
                        }
                        encrypted += intToChar(tmp);
                    }
                    else
                        encrypted += intToChar(position);
                    Debug.LogFormat("[Caesar Cipher #{0}] <Stage {1}> Serial number does not contain vowels. 3 or less batteries on the bomb and Serial Port detected. Encrypted word is {2}", _moduleId, num, encrypted);
                }
                else
                {
                    encrypted = ans;
                    Debug.LogFormat("[Caesar Cipher #{0}] <Stage {1}> Lucky case! Word is not encrypted, so encrypted is {2}", _moduleId, num, ans);
                }
            }
        }
        else
        {
            foreach (char c in ans)
            {
                int position = getPositionFromChar(c);
                position -= key - Info.GetSolvedModuleNames().Count;
                if (position < 0)
                {
                    int tmp = 26;
                    while (position < 0)
                    {
                        position++;
                        tmp--;
                    }
                    encrypted += intToChar(tmp);
                }
                else
                    encrypted += intToChar(position);
            }
            Debug.LogFormat("[Caesar Cipher #{0}] <Stage {1}> Serial number does not contain vowels. 2 or less batteries on the bomb. No Serial Port detected. Encrypted word is {2}", _moduleId, num, encrypted);
        }
            
        Screen.text = encrypted;
    }

    private int getPositionFromChar(char c)
    {
        switch (c)
        {
            case 'A': return 10;
            case 'B': return 23;
            case 'C': return 21;
            case 'D': return 12;
            case 'E': return 2;
            case 'F': return 13;
            case 'G': return 14;
            case 'H': return 15;
            case 'I': return 7;
            case 'J': return 16;
            case 'K': return 17;
            case 'L': return 18;
            case 'M': return 25;
            case 'N': return 24;
            case 'O': return 8;
            case 'P': return 9;
            case 'Q': return 0;
            case 'R': return 3;
            case 'S': return 11;
            case 'T': return 4;
            case 'U': return 6;
            case 'V': return 22;
            case 'W': return 1;
            case 'X': return 20;
            case 'Y': return 5;
            case 'Z': return 19;
        }
        return -1;
    }

    void handlePress(int i)
    {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, btn[i].transform);

        if (!_lightsOn || _isSolved) return;

        UserScreen.text += intToChar(i);
    }

    void ansChk()
    {
        Debug.LogFormat("[Caesar Cipher #{0}] Pressed OK", _moduleId);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, submit.transform);
        submit.AddInteractionPunch();

        Debug.LogFormat("[Caesar Cipher #{0}] Given answer is {1}, expected is {2}", _moduleId, UserScreen.text, ans);

        if (UserScreen.text == ans)
        {
            Debug.LogFormat("[Caesar Cipher #{0}] <Stage{1}> Cleared!", _moduleId, stageCur);
            wordsCounter[stageCur-1].GetComponent<Renderer>().material.color = Color.green;
            stageCur++;
            ans = "";
            if (stageCur > stageAmt)
            {
                Debug.LogFormat("[Caesar Cipher #{0}] Module Solved!", _moduleId);
                Screen.text = "";
                Module.HandlePass();
                _isSolved = true;
            }
            else
            {
                generateStage(stageCur);
                UserScreen.text = "";
            }
        }
        else
        {
            Debug.LogFormat("[Caesar Cipher #{0}] Answer incorrect! Strike and reset!", _moduleId);
            ans = "";
            chosenWords = null;
            Module.HandleStrike();
            Init();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
