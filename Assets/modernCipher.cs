using System.Collections;
using UnityEngine;
using KMHelper;
using System.Linq;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.IO;
using System;

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
    private int _moduleId = 0, totalWords = 0;
    private Dictionary<string, string> chosenWords;
    private bool _isSolved = false, _lightsOn = false;
    private string ans, encrypted;
    private int stageAmt = 3, stageCur = 1;

    private string[] wordsDataBase = { "ABSENT", "ABSTRACT", "ABYSMAL", "ACCIDENT", "ACTIVATE", "ADJACENT", "AFRAID", "AGENDA", "AGONY", "ALCHEMY", "ALCOHOL", "ALIVE", "ALLERGIC", "ALLERGY", "ALPHA", "ALPHABET", "ALREADY", "AMETHYST", "AMNESTY", "AMPERAGE", "ANCIENT", "ANIMALS", "ANIMATE", "ANTHRAX", "ANXIOUS", "AQUARIUM", "AQUARIUS", "ARCADE", "ARRANGE", "ARROW", "ARTEFACT", "ASTERISK", "ATROPHY", "AUDIO", "AUTHOR", "AVOID", "AWESOME", "BALANCE", "BANANA", "BANDIT", "BANKRUPT", "BASKET", "BATTLE", "BAZAAR", "BEARD", "BEAUTY", "BEAVER", "BECOMING", "BEETLE", "BESEECH", "BETWEEN", "BICYCLE", "BIGGER", "BIGGEST", "BIOLOGY", "BIRTHDAY", "BISTRO", "BITES", "BLIGHT", "BLOCKADE", "BLUBBER", "BOMB", "BONOBO", "BOOKS", "BOTTLE", "BRAZIL", "BRIEF", "BROCCOLI", "BROKEN", "BROTHER", "BUBBLE", "BUDGET", "BULKHEAD", "BUMPER", "BUNNY", "BUTTON", "BYTES", "CABLES", "CALIBER", "CAMPAIGN", "CANADA", "CANISTER", "CAPTION", "CAUTION", "CAVITY", "CHALK", "CHAMBER", "CHAMFER", "CHAMPION", "CHANGES", "CHICKEN", "CHILDREN", "CHLORINE", "CHORD", "CHRONIC", "CHURCH", "CINNAMON", "CIVIC", "CLERIC", "CLOCK", "COCOON", "COMBAT", "COMBINE", "COMEDY", "COMICS", "COMMA", "COMMAND", "COMMENT", "COMPOST", "COMPUTER", "CONDOM", "CONFLICT", "CONSIDER", "CONTOUR", "CONTROL", "CORRUPT", "COSTUME", "CRIMINAL", "CRUNCH", "CRYPTIC", "CUBOID", "CYPHER", "DADDY", "DANCER", "DANCING", "DAUGHTER", "DEAD", "DECAPOD", "DECAY", "DECOY", "DEFEAT", "DEFUSER", "DEGREE", "DELAY", "DEMIGOD", "DENTIST", "DESERT", "DESIGN", "DESIRE", "DESSERT", "DETAIL", "DEVELOP", "DEVICE", "DIAMOND", "DICTATE", "DIFFUSE", "DILEMMA", "DINGY", "DINOSAUR", "DISEASE", "DISGUST", "DOCUMENT", "DOUBLED", "DOUBT", "DOWNBEAT", "DRAGON", "DRAWER", "DREAM", "DRINK", "DRUNKEN", "DUNGEON", "DYNASTY", "DYSLEXIA", "ECLIPSE", "ELDRITCH", "EMAIL", "EMULATOR", "ENCRYPT", "ENGLAND", "ENLIST", "ENOUGH", "ENSURE", "EQUALITY", "EQUATION", "ERUPTION", "ETERNITY", "EUPHORIA", "EXACT", "EXCLAIM", "EXHAUST", "EXPERT", "EXPERTLY", "EXPLAIN", "EXPLODES", "FABRIC", "FACTORY", "FADED", "FAINT", "FAIR", "FALSE", "FALTER", "FAMOUS", "FANTASY", "FARM", "FATHER", "FAUCET", "FAULTY", "FEARSOME", "FEAST", "FEBRUARY", "FEINT", "FESTIVAL", "FICTION", "FIGHTER", "FIGURE", "FINISH", "FIREMAN", "FIREWORK", "FIRST", "FIXTURE", "FLAGRANT", "FLAGSHIP", "FLAMINGO", "FLESH", "FLIPPER", "FLUORINE", "FLUSH", "FOREIGN", "FORENSIC", "FRACTAL", "FRAGRANT", "FRANCE", "FRANTIC", "FREAK", "FRICTION", "FRIDAY", "FRIENDLY", "FRIGHTEN", "FUROR", "FUSED", "GARAGE", "GENES", "GENETIC", "GENIUS", "GENTLE", "GLACIER", "GLITCH", "GOAT", "GOLDEN", "GRANULAR", "GRAPHICS", "GRAPHITE", "GRATEFUL", "GRIDLOCK", "GROUND", "GUITAR", "GUMPTION", "HALOGEN", "HARMONY", "HAWK", "HEADACHE", "HEARD", "HEDGEHOG", "HEINOUS", "HERD", "HERETIC", "HEXAGON", "HICCUP", "HIGHWAY", "HOLIDAY", "HOME", "HOMESICK", "HONEST", "HORROR", "HORSE", "HOUSE", "HUGE", "HUMANITY", "HUNGRY", "HYDROGEN", "HYSTERIA", "IMAGINE", "INDUSTRY", "INFAMOUS", "INSIDE", "INTEGRAL", "INTEREST", "IRONCLAD", "ISSUE", "ITALIC", "ITALY", "ITCH", "JAUNDICE", "JEANS", "JEOPARDY", "JOYFUL", "JOYSTICK", "JUICE", "JUNCTURE", "JUNGLE", "JUNKYARD", "JUSTICE", "KEEP", "KEYBOARD", "KILOBYTE", "KILOGRAM", "KINGDOM", "KITCHEN", "KITTEN", "KNIFE", "KRYPTON", "LADYLIKE", "LANGUAGE", "LARGE", "LAUGHTER", "LAUNCH", "LEADERS", "LEARN", "LEAVE", "LEOPARD", "LEVEL", "LIBERAL", "LIBERTY", "LIFEBOAT", "LIGAMENT", "LIGHT", "LIQUID", "LISTEN", "LITTLE", "LOBSTER", "LOGICAL", "LOVE", "LUCKY", "LULLED", "LUNATIC", "LURKS", "MACHINE", "MADAM", "MAGNETIC", "MANAGER", "MANUAL", "MARINA", "MARINE", "MARTIAN", "MASTER", "MATRIX", "MEASURE", "MEATY", "MEDDLE", "MEDICAL", "MENTAL", "MENU", "MEOW", "MERCHANT", "MESSAGE", "MESSES", "METAL", "METHOD", "METTLE", "MILITANT", "MINIM", "MINIMUM", "MIRACLE", "MIRROR", "MISJUDGE", "MISPLACE", "MISSES", "MISTAKE", "MIXTURE", "MNEMONIC", "MOBILE", "MODERN", "MODEST", "MODULE", "MOIST", "MONEY", "MORNING", "MOST", "MOTHER", "MOVIES", "MULTIPLE", "MUNCH", "MUSICAL", "MUSTACHE", "MYSTERY", "MYSTIC", "MYSTIQUE", "MYTHIC", "NARCOTIC", "NASTY", "NATURE", "NAVIGATE", "NETWORK", "NEUTRAL", "NOBELIUM", "NOBODY", "NOISE", "NOTICE", "NOUN", "NUCLEAR", "NUMERAL", "NUTRIENT", "NYMPH", "OBELISK", "OBSTACLE", "OBVIOUS", "OCTOPUS", "OFFSET", "OMEGA", "OPAQUE", "OPINION", "ORANGE", "ORGANIC", "OUCH", "OUTBREAK", "OUTDO", "OVERCAST", "OVERLAPS", "PACKAGE", "PADLOCK", "PANCAKE", "PANDA", "PANIC", "PAPER", "PAPERS", "PARENT", "PARK", "PARTICLE", "PASSIVE", "PATENTED", "PATHETIC", "PATIENT", "PEACE", "PEASANT", "PENALTY", "PENCIL", "PENGUIN", "PERFECT", "PERSON", "PERSUADE", "PERUSING", "PHONE", "PHYSICAL", "PIANO", "PICTURE", "PIGLET", "PILFER", "PILLAGE", "PINCH", "PIRATE", "PITCHER", "PIZZA", "PLANE", "PLANET", "PLATONIC", "PLAYER", "PLEASE", "PLUCKY", "PLUNDER", "PLURALS", "POCKET", "POLICE", "PORTRAIT", "POTATO", "POTENTLY", "POUNCE", "POVERTY", "PRACTICE", "PREDICT", "PREFECT", "PREMIUM", "PRESENT", "PRINCE", "PRINTER", "PRISON", "PROFIT", "PROMISE", "PROPHET", "PROTEIN", "PROVINCE", "PSALM", "PSYCHIC", "PUDDLE", "PUNCHBAG", "PUNGENT", "PUNISH", "PURCHASE", "QUAGMIRE", "QUALIFY", "QUANTIFY", "QUANTIZE", "QUARTER", "QUERYING", "QUEUE", "QUICHE", "QUICK", "RABBIT", "RACOON", "RADAR", "RADICAL", "RAINBOW", "RANDOM", "RATTLE", "RAVENOUS", "REASON", "REBUKE", "REFINE", "REGULAR", "REINDEER", "REQUEST", "RESORT", "RESPECT", "RETIRE", "REVOLT", "REWARD", "RHAPSODY", "RHENIUM", "RHODIUM", "RHOMBOID", "RHYME", "RHYTHM", "RIDICULE", "ROADWORK", "ROAR", "ROAST", "ROOM", "ROOSTER", "ROSTER", "ROTOR", "ROTUNDA", "ROYAL", "RULER", "RURAL", "SAILOR", "SAINTED", "SALES", "SALLY", "SATISFY", "SAUNTER", "SCALE", "SCANDAL", "SCHEDULE", "SCHOOL", "SCIENCE", "SCRATCH", "SCREEN", "SENSIBLE", "SEPARATE", "SERIOUS", "SEVERAL", "SHAMPOO", "SHARES", "SHELTER", "SHIFT", "SHIP", "SHIRT", "SHIVER", "SHORTEN", "SHOWCASE", "SHUFFLE", "SILENT", "SIMILAR", "SISTER", "SIXTH", "SIXTY", "SKATER", "SKYWARD", "SLANDER", "SLAYER", "SLEEK", "SLIPPER", "SMART", "SMEARED", "SOCCER", "SOCIETY", "SOURCE", "SPAIN", "SPARE", "SPARK", "SPATULA", "SPEAKER", "SPECIAL", "SPECTATE", "SPECTRUM", "SPICY", "SPINACH", "SPIRAL", "SPLENDID", "SPLINTER", "SPRAYED", "SPREAD", "SPRING", "SQUADRON", "SQUANDER", "SQUASH", "SQUIB", "SQUID", "SQUISH", "STAKE", "STALKING", "STEAK", "STEAM", "STICKER", "STINKY", "STOCKING", "STONE", "STORE", "STORMY", "STRANGE", "STRIKE", "STUTTER", "SUBWAY", "SUFFER", "SUPREME", "SURF", "SURPLUS", "SURVEY", "SWITCH", "SYMBOL", "SYSTEM", "SYSTEMIC", "TABLE", "TADPOLE", "TALKING", "TANGLE", "TANK", "TAPEWORM", "TARGET", "TAROT", "TEACH", "TEAMWORK", "TERMINAL", "TERMINUS", "TERROR", "TESTIFY", "THEIR", "THERE", "THICK", "THIEF", "THINK", "THROAT", "THROUGH", "THUNDER", "THYME", "TICKET", "TIME", "TOASTER", "TOMATO", "TONE", "TORQUE", "TORTOISE", "TOUCHY", "TOUPE", "TOWER", "TRANSFIX", "TRANSIT", "TRASH", "TRAUMA", "TREASON", "TREASURE", "TRICK", "TRIPOD", "TROUBLE", "TRUCK", "TRUMPET", "TURTLE", "TWINKLE", "UGLY", "ULTRA", "UMBRELLA", "UNDERWAY", "UNIQUE", "UNKNOWN", "UNSTEADY", "UNTOWARD", "UNWASHED", "UPGRADE", "URBAN", "USED", "USELESS", "UTOPIA", "VACUUM", "VAMPIRE", "VANISH", "VANQUISH", "VARIOUS", "VAST", "VELOCITY", "VENDOR", "VERB", "VERBATIM", "VERDICT", "VEXATION", "VICIOUS", "VICTIM", "VICTORY", "VIDEO", "VIEW", "VIKING", "VILLAGE", "VIOLENT", "VIOLIN", "VIRULENT", "VISCERAL", "VISION", "VOLATILE", "VOLTAGE", "VORTEX", "VULGAR", "WARDEN", "WARLOCK", "WARNING", "WEALTH", "WEAPON", "WEDDING", "WEIGHT", "WHACK", "WHARF", "WHAT", "WHEN", "WHISK", "WHISTLE", "WICKED", "WINDOW", "WINTER", "WITNESS", "WIZARD", "WRENCH", "WRETCH", "WRINKLE", "WRITER", "XANTHOUS", "YACHT", "YARN", "YAWN", "YEAH", "YEARLONG", "YEARN", "YEOMAN", "YODEL", "YOGA", "YONDER", "YOUNGEST", "YOURSELF", "ZEALOT", "ZEBRA", "ZENITH", "ZITHER", "ZODIAC", "ZOMBIE" };

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
        string qwerty = "QWERTYUIOPASDFGHJKLZXCVBNM";
        i %= 26;
        if (i < 0) i += 26;
        return qwerty.Substring(i, 1);
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
        Debug.LogFormat("[Modern Cipher #{0}] totalWords = {1}", _moduleId, totalWords);
        wordsCounter[0].GetComponent<Renderer>().material = ledsMat;
        wordsCounter[1].GetComponent<Renderer>().material = ledsMat;
        wordsCounter[2].GetComponent<Renderer>().material = ledsMat;
        stageCur = 1;
        generateStage(1);
    }

    private void generateStage(int num)
    {
        Debug.LogFormat("[Modern Cipher #{0}] <Stage {1}> START", _moduleId, num);

        do
            ans = wordsDataBase[Random.Range(0, wordsDataBase.Length)];
        while (chosenWords.Values.Contains(ans));
        chosenWords.Add("Stage"+stageCur, ans);
        encrypted = "";
        Debug.LogFormat("[Modern Cipher #{0}] <Stage {1}> Picked word from array is {2}", _moduleId, num, ans);

        int key = 0;
        string numbers = "";

        foreach (int number in Info.GetSerialNumberNumbers())
        {
            numbers += number.ToString() + "+";
            key += number;
        }

        numbers += (Info.GetStrikes()).ToString();
        key += Info.GetStrikes();

        Debug.LogFormat("[Modern Cipher #{0}] <Stage {1}> Numbers in serial and number of strikes: {2} = {3} <= this is the key", _moduleId, num, numbers, key);

        if (Info.GetSerialNumberLetters().Any("AEIOU".Contains))
        {
            foreach (char c in ans)
            {
                int position = getPositionFromChar(c);
                position += key;
                encrypted += intToChar(position);
            }
            Debug.LogFormat("[Modern Cipher #{0}] <Stage {1}> Serial number contains at least a vowel. Encrypted word is {2}", _moduleId, num, encrypted);
        }
        else if (Info.GetBatteryCount() > 3)
        {
            foreach (char c in ans)
            {
                int position = getPositionFromChar(c);
                position -= key;
                encrypted += intToChar(position);
            }
            Debug.LogFormat("[Modern Cipher #{0}] <Stage {1}> Serial number does not contain vowels. More than 3 batteries on the bomb. Encrypted word is {2}", _moduleId, num, encrypted);
        }
        else if (Info.IsPortPresent(KMBombInfoExtensions.KnownPortType.Serial))
        {
            foreach (char c in ans)
            {
                int position = getPositionFromChar(c);
                if (stageCur != 1)
                {
                    position += key + chosenWords["Stage" + (stageCur - 1).ToString()].Count(); //sommo alla chiave il numero di lettere della parola precedente
                    encrypted += intToChar(position);
                }
                else
                {
                    position += key;
                    encrypted += intToChar(position);
                }
            }
            Debug.LogFormat("[Modern Cipher #{0}] <Stage {1}> Serial number does not contain vowels. 3 or less batteries on the bomb and Serial Port detected. Encrypted word is {2}", _moduleId, num, encrypted);
        }
        else
        {
            Debug.LogFormat("[Modern Cipher #{0}] <Stage {1}> The number of solved modules is {2}", _moduleId, num, Info.GetSolvedModuleNames().Count);
            foreach (char c in ans)
            {
                int position = getPositionFromChar(c);
                position -= (key + Info.GetSolvedModuleNames().Count);
                encrypted += intToChar(position);
            }
            Debug.LogFormat("[Modern Cipher #{0}] <Stage {1}> Serial number does not contain vowels. 2 or less batteries on the bomb. No Serial Port detected. Encrypted word is {2}", _moduleId, num, encrypted);
        }
            
        Screen.text = encrypted;
    }

    private int getPositionFromChar(char c)
    {
        return "QWERTYUIOPASDFGHJKLZXCVBNM".IndexOf(c);
    }

    void handlePress(int i)
    {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, btn[i].transform);

        if (!_lightsOn || _isSolved) return;

        UserScreen.text += intToChar(i);
    }

    void ansChk()
    {
        Debug.LogFormat("[Modern Cipher #{0}] Pressed OK", _moduleId);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, submit.transform);
        submit.AddInteractionPunch();

        Debug.LogFormat("[Modern Cipher #{0}] Given answer is {1}, expected is {2}", _moduleId, UserScreen.text, ans);

        if (UserScreen.text == ans)
        {
            Debug.LogFormat("[Modern Cipher #{0}] <Stage{1}> Cleared!", _moduleId, stageCur);
            wordsCounter[stageCur-1].GetComponent<Renderer>().material.color = Color.green;
            stageCur++;
            ans = "";
            if (stageCur > stageAmt)
            {
                Debug.LogFormat("[Modern Cipher #{0}] Module Solved!", _moduleId);
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
            Debug.LogFormat("[Modern Cipher #{0}] Answer incorrect! Strike and reset!", _moduleId);
            ans = "";
            chosenWords = null;
            Module.HandleStrike();
            Init();
        }
    }

    private string TwitchHelpMessage = "Submit the decrypted word with !{0} submit printer.";
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] split = command.ToUpperInvariant().Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
        if (split.Length != 2 || !split[0].Equals("SUBMIT")) yield break;
        int[] buttons = split[1].Select(getPositionFromChar).ToArray();
        if (buttons.Any(x => x < 0)) yield break;
        yield return null;
        erase.OnInteract();
        yield return new WaitForSeconds(0.1f);
        foreach (int button in buttons)
        {
            btn[button].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        submit.OnInteract();
        yield return new WaitForSeconds(0.1f);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
