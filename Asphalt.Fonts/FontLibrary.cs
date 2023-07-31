using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace Asphalt.Controls
{
    [SuppressUnmanagedCodeSecurity]
    internal class WinGdi
    {
        [DllImport( "gdi32.dll" )]
        internal static extern IntPtr AddFontMemResourceEx( IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts );

        [DllImport( "gdi32.dll" )]
        internal static extern bool RemoveFontMemResourceEx( IntPtr pHandle );
    }

    // the styles we support from the custom font
    public enum FontChoice
    {
        Regular,
        Light,
        SemiBold,
        CondensedRegular,
        Monospace,
        Rigid,

        Italic,
        LightItalic,

        Bold,
        BoldItalic,
        SemiBoldItalic,

        CondensedItalic,
        CondensedBold,
    }

    // preset sizes so that size values aren't scattered in form code
    public enum FontSize
    {
        Tiny,
        Smaller,
        Normal,
        Larger,
        Title
    }

    // unicode values for useful icons from Font Awesome 5
    // https://fontawesome.com/icons?d=gallery&m=free
    public enum IconID
    {
        None                = 0,

        Num0,
        Num1,
        Num2,
        Num3,
        Num4,
        Num5,
        Num6,
        Num7,
        Num8,
        Num9,
        A,
        AddressBook,
        AddressCard,
        AlignCenter,
        AlignJustify,
        AlignLeft,
        AlignRight,
        Anchor,
        AnchorCircleCheck,
        AnchorCircleExclamation,
        AnchorCircleXmark,
        AnchorLock,
        AngleDown,
        AngleLeft,
        AngleRight,
        AngleUp,
        AnglesDown,
        AnglesLeft,
        AnglesRight,
        AnglesUp,
        Ankh,
        AppleWhole,
        Archway,
        ArrowDown,
        ArrowDown19,
        ArrowDown91,
        ArrowDownAZ,
        ArrowDownLong,
        ArrowDownShortWide,
        ArrowDownUpAcrossLine,
        ArrowDownUpLock,
        ArrowDownWideShort,
        ArrowDownZA,
        ArrowLeft,
        ArrowLeftLong,
        ArrowPointer,
        ArrowRight,
        ArrowRightArrowLeft,
        ArrowRightFromBracket,
        ArrowRightLong,
        ArrowRightToBracket,
        ArrowRightToCity,
        ArrowRotateLeft,
        ArrowRotateRight,
        ArrowTrendDown,
        ArrowTrendUp,
        ArrowTurnDown,
        ArrowTurnUp,
        ArrowUp,
        ArrowUp19,
        ArrowUp91,
        ArrowUpAZ,
        ArrowUpFromBracket,
        ArrowUpFromGroundWater,
        ArrowUpFromWaterPump,
        ArrowUpLong,
        ArrowUpRightDots,
        ArrowUpRightFromSquare,
        ArrowUpShortWide,
        ArrowUpWideShort,
        ArrowUpZA,
        ArrowsDownToLine,
        ArrowsDownToPeople,
        ArrowsLeftRight,
        ArrowsLeftRightToLine,
        ArrowsRotate,
        ArrowsSpin,
        ArrowsSplitUpAndLeft,
        ArrowsToCircle,
        ArrowsToDot,
        ArrowsToEye,
        ArrowsTurnRight,
        ArrowsTurnToDots,
        ArrowsUpDown,
        ArrowsUpDownLeftRight,
        ArrowsUpToLine,
        Asterisk,
        At,
        Atom,
        AudioDescription,
        AustralSign,
        Award,
        B,
        Baby,
        BabyCarriage,
        Backward,
        BackwardFast,
        BackwardStep,
        Bacon,
        Bacteria,
        Bacterium,
        BagShopping,
        Bahai,
        BahtSign,
        Ban,
        BanSmoking,
        Bandage,
        BangladeshiTakaSign,
        Barcode,
        Bars,
        BarsProgress,
        BarsStaggered,
        Baseball,
        BaseballBatBall,
        BasketShopping,
        Basketball,
        Bath,
        BatteryEmpty,
        BatteryFull,
        BatteryHalf,
        BatteryQuarter,
        BatteryThreeQuarters,
        Bed,
        BedPulse,
        BeerMugEmpty,
        Bell,
        BellConcierge,
        BellSlash,
        BezierCurve,
        Bicycle,
        Binoculars,
        Biohazard,
        BitcoinSign,
        Blender,
        BlenderPhone,
        Blog,
        Bold,
        Bolt,
        BoltLightning,
        Bomb,
        Bone,
        Bong,
        Book,
        BookAtlas,
        BookBible,
        BookBookmark,
        BookJournalWhills,
        BookMedical,
        BookOpen,
        BookOpenReader,
        BookQuran,
        BookSkull,
        BookTanakh,
        Bookmark,
        BorderAll,
        BorderNone,
        BorderTopLeft,
        BoreHole,
        BottleDroplet,
        BottleWater,
        BowlFood,
        BowlRice,
        BowlingBall,
        Box,
        BoxArchive,
        BoxOpen,
        BoxTissue,
        BoxesPacking,
        BoxesStacked,
        Braille,
        Brain,
        BrazilianRealSign,
        BreadSlice,
        Bridge,
        BridgeCircleCheck,
        BridgeCircleExclamation,
        BridgeCircleXmark,
        BridgeLock,
        BridgeWater,
        Briefcase,
        BriefcaseMedical,
        Broom,
        BroomBall,
        Brush,
        Bucket,
        Bug,
        BugSlash,
        Bugs,
        Building,
        BuildingCircleArrowRight,
        BuildingCircleCheck,
        BuildingCircleExclamation,
        BuildingCircleXmark,
        BuildingColumns,
        BuildingFlag,
        BuildingLock,
        BuildingNgo,
        BuildingShield,
        BuildingUn,
        BuildingUser,
        BuildingWheat,
        Bullhorn,
        Bullseye,
        Burger,
        Burst,
        Bus,
        BusSimple,
        BusinessTime,
        C,
        CableCar,
        CakeCandles,
        Calculator,
        Calendar,
        CalendarCheck,
        CalendarDay,
        CalendarDays,
        CalendarMinus,
        CalendarPlus,
        CalendarWeek,
        CalendarXmark,
        Camera,
        CameraRetro,
        CameraRotate,
        Campground,
        CandyCane,
        Cannabis,
        Capsules,
        Car,
        CarBattery,
        CarBurst,
        CarOn,
        CarRear,
        CarSide,
        CarTunnel,
        Caravan,
        CaretDown,
        CaretLeft,
        CaretRight,
        CaretUp,
        Carrot,
        CartArrowDown,
        CartFlatbed,
        CartFlatbedSuitcase,
        CartPlus,
        CartShopping,
        CashRegister,
        Cat,
        CediSign,
        CentSign,
        Certificate,
        Chair,
        Chalkboard,
        ChalkboardUser,
        ChampagneGlasses,
        ChargingStation,
        ChartArea,
        ChartBar,
        ChartColumn,
        ChartGantt,
        ChartLine,
        ChartPie,
        ChartSimple,
        Check,
        CheckDouble,
        CheckToSlot,
        Cheese,
        Chess,
        ChessBishop,
        ChessBoard,
        ChessKing,
        ChessKnight,
        ChessPawn,
        ChessQueen,
        ChessRook,
        ChevronDown,
        ChevronLeft,
        ChevronRight,
        ChevronUp,
        Child,
        ChildCombatant,
        ChildDress,
        ChildReaching,
        Children,
        Church,
        Circle,
        CircleArrowDown,
        CircleArrowLeft,
        CircleArrowRight,
        CircleArrowUp,
        CircleCheck,
        CircleChevronDown,
        CircleChevronLeft,
        CircleChevronRight,
        CircleChevronUp,
        CircleDollarToSlot,
        CircleDot,
        CircleDown,
        CircleExclamation,
        CircleH,
        CircleHalfStroke,
        CircleInfo,
        CircleLeft,
        CircleMinus,
        CircleNodes,
        CircleNotch,
        CirclePause,
        CirclePlay,
        CirclePlus,
        CircleQuestion,
        CircleRadiation,
        CircleRight,
        CircleStop,
        CircleUp,
        CircleUser,
        CircleXmark,
        City,
        Clapperboard,
        Clipboard,
        ClipboardCheck,
        ClipboardList,
        ClipboardQuestion,
        ClipboardUser,
        Clock,
        ClockRotateLeft,
        Clone,
        ClosedCaptioning,
        Cloud,
        CloudArrowDown,
        CloudArrowUp,
        CloudBolt,
        CloudMeatball,
        CloudMoon,
        CloudMoonRain,
        CloudRain,
        CloudShowersHeavy,
        CloudShowersWater,
        CloudSun,
        CloudSunRain,
        Clover,
        Code,
        CodeBranch,
        CodeCommit,
        CodeCompare,
        CodeFork,
        CodeMerge,
        CodePullRequest,
        Coins,
        ColonSign,
        Comment,
        CommentDollar,
        CommentDots,
        CommentMedical,
        CommentSlash,
        CommentSms,
        Comments,
        CommentsDollar,
        CompactDisc,
        Compass,
        CompassDrafting,
        Compress,
        Computer,
        ComputerMouse,
        Cookie,
        CookieBite,
        Copy,
        Copyright,
        Couch,
        Cow,
        CreditCard,
        Crop,
        CropSimple,
        Cross,
        Crosshairs,
        Crow,
        Crown,
        Crutch,
        CruzeiroSign,
        Cube,
        Cubes,
        CubesStacked,
        D,
        Database,
        DeleteLeft,
        Democrat,
        Desktop,
        Dharmachakra,
        DiagramNext,
        DiagramPredecessor,
        DiagramProject,
        DiagramSuccessor,
        Diamond,
        DiamondTurnRight,
        Dice,
        DiceD20,
        DiceD6,
        DiceFive,
        DiceFour,
        DiceOne,
        DiceSix,
        DiceThree,
        DiceTwo,
        Disease,
        Display,
        Divide,
        Dna,
        Dog,
        DollarSign,
        Dolly,
        DongSign,
        DoorClosed,
        DoorOpen,
        Dove,
        DownLeftAndUpRightToCenter,
        DownLong,
        Download,
        Dragon,
        DrawPolygon,
        Droplet,
        DropletSlash,
        Drum,
        DrumSteelpan,
        DrumstickBite,
        Dumbbell,
        Dumpster,
        DumpsterFire,
        Dungeon,
        E,
        EarDeaf,
        EarListen,
        EarthAfrica,
        EarthAmericas,
        EarthAsia,
        EarthEurope,
        EarthOceania,
        Egg,
        Eject,
        Elevator,
        Ellipsis,
        EllipsisVertical,
        Envelope,
        EnvelopeCircleCheck,
        EnvelopeOpen,
        EnvelopeOpenText,
        EnvelopesBulk,
        Equals,
        Eraser,
        Ethernet,
        EuroSign,
        Exclamation,
        Expand,
        Explosion,
        Eye,
        EyeDropper,
        EyeLowVision,
        EyeSlash,
        F,
        FaceAngry,
        FaceDizzy,
        FaceFlushed,
        FaceFrown,
        FaceFrownOpen,
        FaceGrimace,
        FaceGrin,
        FaceGrinBeam,
        FaceGrinBeamSweat,
        FaceGrinHearts,
        FaceGrinSquint,
        FaceGrinSquintTears,
        FaceGrinStars,
        FaceGrinTears,
        FaceGrinTongue,
        FaceGrinTongueSquint,
        FaceGrinTongueWink,
        FaceGrinWide,
        FaceGrinWink,
        FaceKiss,
        FaceKissBeam,
        FaceKissWinkHeart,
        FaceLaugh,
        FaceLaughBeam,
        FaceLaughSquint,
        FaceLaughWink,
        FaceMeh,
        FaceMehBlank,
        FaceRollingEyes,
        FaceSadCry,
        FaceSadTear,
        FaceSmile,
        FaceSmileBeam,
        FaceSmileWink,
        FaceSurprise,
        FaceTired,
        Fan,
        Faucet,
        FaucetDrip,
        Fax,
        Feather,
        FeatherPointed,
        Ferry,
        File,
        FileArrowDown,
        FileArrowUp,
        FileAudio,
        FileCircleCheck,
        FileCircleExclamation,
        FileCircleMinus,
        FileCirclePlus,
        FileCircleQuestion,
        FileCircleXmark,
        FileCode,
        FileContract,
        FileCsv,
        FileExcel,
        FileExport,
        FileImage,
        FileImport,
        FileInvoice,
        FileInvoiceDollar,
        FileLines,
        FileMedical,
        FilePdf,
        FilePen,
        FilePowerpoint,
        FilePrescription,
        FileShield,
        FileSignature,
        FileVideo,
        FileWaveform,
        FileWord,
        FileZipper,
        Fill,
        FillDrip,
        Film,
        Filter,
        FilterCircleDollar,
        FilterCircleXmark,
        Fingerprint,
        Fire,
        FireBurner,
        FireExtinguisher,
        FireFlameCurved,
        FireFlameSimple,
        Fish,
        FishFins,
        Flag,
        FlagCheckered,
        FlagUsa,
        Flask,
        FlaskVial,
        FloppyDisk,
        FlorinSign,
        Folder,
        FolderClosed,
        FolderMinus,
        FolderOpen,
        FolderPlus,
        FolderTree,
        Font,
        FontAwesome,
        Football,
        Forward,
        ForwardFast,
        ForwardStep,
        FrancSign,
        Frog,
        Futbol,
        G,
        Gamepad,
        GasPump,
        Gauge,
        GaugeHigh,
        GaugeSimple,
        GaugeSimpleHigh,
        Gavel,
        Gear,
        Gears,
        Gem,
        Genderless,
        Ghost,
        Gift,
        Gifts,
        GlassWater,
        GlassWaterDroplet,
        Glasses,
        Globe,
        GolfBallTee,
        Gopuram,
        GraduationCap,
        GreaterThan,
        GreaterThanEqual,
        Grip,
        GripLines,
        GripLinesVertical,
        GripVertical,
        GroupArrowsRotate,
        GuaraniSign,
        Guitar,
        Gun,
        H,
        Hammer,
        Hamsa,
        Hand,
        HandBackFist,
        HandDots,
        HandFist,
        HandHolding,
        HandHoldingDollar,
        HandHoldingDroplet,
        HandHoldingHand,
        HandHoldingHeart,
        HandHoldingMedical,
        HandLizard,
        HandMiddleFinger,
        HandPeace,
        HandPointDown,
        HandPointLeft,
        HandPointRight,
        HandPointUp,
        HandPointer,
        HandScissors,
        HandSparkles,
        HandSpock,
        Handcuffs,
        Hands,
        HandsAslInterpreting,
        HandsBound,
        HandsBubbles,
        HandsClapping,
        HandsHolding,
        HandsHoldingChild,
        HandsHoldingCircle,
        HandsPraying,
        Handshake,
        HandshakeAngle,
        HandshakeSimple,
        HandshakeSimpleSlash,
        HandshakeSlash,
        Hanukiah,
        HardDrive,
        Hashtag,
        HatCowboy,
        HatCowboySide,
        HatWizard,
        HeadSideCough,
        HeadSideCoughSlash,
        HeadSideMask,
        HeadSideVirus,
        Heading,
        Headphones,
        HeadphonesSimple,
        Headset,
        Heart,
        HeartCircleBolt,
        HeartCircleCheck,
        HeartCircleExclamation,
        HeartCircleMinus,
        HeartCirclePlus,
        HeartCircleXmark,
        HeartCrack,
        HeartPulse,
        Helicopter,
        HelicopterSymbol,
        HelmetSafety,
        HelmetUn,
        Highlighter,
        HillAvalanche,
        HillRockslide,
        Hippo,
        HockeyPuck,
        HollyBerry,
        Horse,
        HorseHead,
        Hospital,
        HospitalUser,
        HotTubPerson,
        Hotdog,
        Hotel,
        Hourglass,
        HourglassEnd,
        HourglassHalf,
        HourglassStart,
        House,
        HouseChimney,
        HouseChimneyCrack,
        HouseChimneyMedical,
        HouseChimneyUser,
        HouseChimneyWindow,
        HouseCircleCheck,
        HouseCircleExclamation,
        HouseCircleXmark,
        HouseCrack,
        HouseFire,
        HouseFlag,
        HouseFloodWater,
        HouseFloodWaterCircleArrowRight,
        HouseLaptop,
        HouseLock,
        HouseMedical,
        HouseMedicalCircleCheck,
        HouseMedicalCircleExclamation,
        HouseMedicalCircleXmark,
        HouseMedicalFlag,
        HouseSignal,
        HouseTsunami,
        HouseUser,
        HryvniaSign,
        Hurricane,
        I,
        ICursor,
        IceCream,
        Icicles,
        Icons,
        IdBadge,
        IdCard,
        IdCardClip,
        Igloo,
        Image,
        ImagePortrait,
        Images,
        Inbox,
        Indent,
        IndianRupeeSign,
        Industry,
        Infinity,
        Info,
        Italic,
        J,
        Jar,
        JarWheat,
        Jedi,
        JetFighter,
        JetFighterUp,
        Joint,
        JugDetergent,
        K,
        Kaaba,
        Key,
        Keyboard,
        Khanda,
        KipSign,
        KitMedical,
        KitchenSet,
        KiwiBird,
        L,
        LandMineOn,
        Landmark,
        LandmarkDome,
        LandmarkFlag,
        Language,
        Laptop,
        LaptopCode,
        LaptopFile,
        LaptopMedical,
        LariSign,
        LayerGroup,
        Leaf,
        LeftLong,
        LeftRight,
        Lemon,
        LessThan,
        LessThanEqual,
        LifeRing,
        Lightbulb,
        LinesLeaning,
        Link,
        LinkSlash,
        LiraSign,
        List,
        ListCheck,
        ListOl,
        ListUl,
        LitecoinSign,
        LocationArrow,
        LocationCrosshairs,
        LocationDot,
        LocationPin,
        LocationPinLock,
        Lock,
        LockOpen,
        Locust,
        Lungs,
        LungsVirus,
        M,
        Magnet,
        MagnifyingGlass,
        MagnifyingGlassArrowRight,
        MagnifyingGlassChart,
        MagnifyingGlassDollar,
        MagnifyingGlassLocation,
        MagnifyingGlassMinus,
        MagnifyingGlassPlus,
        ManatSign,
        Map,
        MapLocation,
        MapLocationDot,
        MapPin,
        Marker,
        Mars,
        MarsAndVenus,
        MarsAndVenusBurst,
        MarsDouble,
        MarsStroke,
        MarsStrokeRight,
        MarsStrokeUp,
        MartiniGlass,
        MartiniGlassCitrus,
        MartiniGlassEmpty,
        Mask,
        MaskFace,
        MaskVentilator,
        MasksTheater,
        MattressPillow,
        Maximize,
        Medal,
        Memory,
        Menorah,
        Mercury,
        Message,
        Meteor,
        Microchip,
        Microphone,
        MicrophoneLines,
        MicrophoneLinesSlash,
        MicrophoneSlash,
        Microscope,
        MillSign,
        Minimize,
        Minus,
        Mitten,
        Mobile,
        MobileButton,
        MobileRetro,
        MobileScreen,
        MobileScreenButton,
        MoneyBill,
        MoneyBill1,
        MoneyBill1Wave,
        MoneyBillTransfer,
        MoneyBillTrendUp,
        MoneyBillWave,
        MoneyBillWheat,
        MoneyBills,
        MoneyCheck,
        MoneyCheckDollar,
        Monument,
        Moon,
        MortarPestle,
        Mosque,
        Mosquito,
        MosquitoNet,
        Motorcycle,
        Mound,
        Mountain,
        MountainCity,
        MountainSun,
        MugHot,
        MugSaucer,
        Music,
        N,
        NairaSign,
        NetworkWired,
        Neuter,
        Newspaper,
        NotEqual,
        Notdef,
        NoteSticky,
        NotesMedical,
        O,
        ObjectGroup,
        ObjectUngroup,
        OilCan,
        OilWell,
        Om,
        Otter,
        Outdent,
        P,
        Pager,
        PaintRoller,
        Paintbrush,
        Palette,
        Pallet,
        Panorama,
        PaperPlane,
        Paperclip,
        ParachuteBox,
        Paragraph,
        Passport,
        Paste,
        Pause,
        Paw,
        Peace,
        Pen,
        PenClip,
        PenFancy,
        PenNib,
        PenRuler,
        PenToSquare,
        Pencil,
        PeopleArrows,
        PeopleCarryBox,
        PeopleGroup,
        PeopleLine,
        PeoplePulling,
        PeopleRobbery,
        PeopleRoof,
        PepperHot,
        Percent,
        Person,
        PersonArrowDownToLine,
        PersonArrowUpFromLine,
        PersonBiking,
        PersonBooth,
        PersonBreastfeeding,
        PersonBurst,
        PersonCane,
        PersonChalkboard,
        PersonCircleCheck,
        PersonCircleExclamation,
        PersonCircleMinus,
        PersonCirclePlus,
        PersonCircleQuestion,
        PersonCircleXmark,
        PersonDigging,
        PersonDotsFromLine,
        PersonDress,
        PersonDressBurst,
        PersonDrowning,
        PersonFalling,
        PersonFallingBurst,
        PersonHalfDress,
        PersonHarassing,
        PersonHiking,
        PersonMilitaryPointing,
        PersonMilitaryRifle,
        PersonMilitaryToPerson,
        PersonPraying,
        PersonPregnant,
        PersonRays,
        PersonRifle,
        PersonRunning,
        PersonShelter,
        PersonSkating,
        PersonSkiing,
        PersonSkiingNordic,
        PersonSnowboarding,
        PersonSwimming,
        PersonThroughWindow,
        PersonWalking,
        PersonWalkingArrowLoopLeft,
        PersonWalkingArrowRight,
        PersonWalkingDashedLineArrowRight,
        PersonWalkingLuggage,
        PersonWalkingWithCane,
        PesetaSign,
        PesoSign,
        Phone,
        PhoneFlip,
        PhoneSlash,
        PhoneVolume,
        PhotoFilm,
        PiggyBank,
        Pills,
        PizzaSlice,
        PlaceOfWorship,
        Plane,
        PlaneArrival,
        PlaneCircleCheck,
        PlaneCircleExclamation,
        PlaneCircleXmark,
        PlaneDeparture,
        PlaneLock,
        PlaneSlash,
        PlaneUp,
        PlantWilt,
        PlateWheat,
        Play,
        Plug,
        PlugCircleBolt,
        PlugCircleCheck,
        PlugCircleExclamation,
        PlugCircleMinus,
        PlugCirclePlus,
        PlugCircleXmark,
        Plus,
        PlusMinus,
        Podcast,
        Poo,
        PooStorm,
        Poop,
        PowerOff,
        Prescription,
        PrescriptionBottle,
        PrescriptionBottleMedical,
        Print,
        PumpMedical,
        PumpSoap,
        PuzzlePiece,
        Q,
        Qrcode,
        Question,
        QuoteLeft,
        QuoteRight,
        R,
        Radiation,
        Radio,
        Rainbow,
        RankingStar,
        Receipt,
        RecordVinyl,
        RectangleAd,
        RectangleList,
        RectangleXmark,
        Recycle,
        Registered,
        Repeat,
        Reply,
        ReplyAll,
        Restroom,
        Retweet,
        Ribbon,
        RightFromBracket,
        RightLeft,
        RightLong,
        RightToBracket,
        Ring,
        Road,
        RoadBarrier,
        RoadBridge,
        RoadCircleCheck,
        RoadCircleExclamation,
        RoadCircleXmark,
        RoadLock,
        RoadSpikes,
        Robot,
        Rocket,
        Rotate,
        RotateLeft,
        RotateRight,
        Route,
        Rss,
        RubleSign,
        Rug,
        Ruler,
        RulerCombined,
        RulerHorizontal,
        RulerVertical,
        RupeeSign,
        RupiahSign,
        S,
        SackDollar,
        SackXmark,
        Sailboat,
        Satellite,
        SatelliteDish,
        ScaleBalanced,
        ScaleUnbalanced,
        ScaleUnbalancedFlip,
        School,
        SchoolCircleCheck,
        SchoolCircleExclamation,
        SchoolCircleXmark,
        SchoolFlag,
        SchoolLock,
        Scissors,
        Screwdriver,
        ScrewdriverWrench,
        Scroll,
        ScrollTorah,
        SdCard,
        Section,
        Seedling,
        Server,
        Shapes,
        Share,
        ShareFromSquare,
        ShareNodes,
        SheetPlastic,
        ShekelSign,
        Shield,
        ShieldCat,
        ShieldDog,
        ShieldHalved,
        ShieldHeart,
        ShieldVirus,
        Ship,
        Shirt,
        ShoePrints,
        Shop,
        ShopLock,
        ShopSlash,
        Shower,
        Shrimp,
        Shuffle,
        ShuttleSpace,
        SignHanging,
        Signal,
        Signature,
        SignsPost,
        SimCard,
        Sink,
        Sitemap,
        Skull,
        SkullCrossbones,
        Slash,
        Sleigh,
        Sliders,
        Smog,
        Smoking,
        Snowflake,
        Snowman,
        Snowplow,
        Soap,
        Socks,
        SolarPanel,
        Sort,
        SortDown,
        SortUp,
        Spa,
        SpaghettiMonsterFlying,
        SpellCheck,
        Spider,
        Spinner,
        Splotch,
        Spoon,
        SprayCan,
        SprayCanSparkles,
        Square,
        SquareArrowUpRight,
        SquareCaretDown,
        SquareCaretLeft,
        SquareCaretRight,
        SquareCaretUp,
        SquareCheck,
        SquareEnvelope,
        SquareFull,
        SquareH,
        SquareMinus,
        SquareNfi,
        SquareParking,
        SquarePen,
        SquarePersonConfined,
        SquarePhone,
        SquarePhoneFlip,
        SquarePlus,
        SquarePollHorizontal,
        SquarePollVertical,
        SquareRootVariable,
        SquareRss,
        SquareShareNodes,
        SquareUpRight,
        SquareVirus,
        SquareXmark,
        StaffSnake,
        Stairs,
        Stamp,
        Stapler,
        Star,
        StarAndCrescent,
        StarHalf,
        StarHalfStroke,
        StarOfDavid,
        StarOfLife,
        SterlingSign,
        Stethoscope,
        Stop,
        Stopwatch,
        Stopwatch20,
        Store,
        StoreSlash,
        StreetView,
        Strikethrough,
        Stroopwafel,
        Subscript,
        Suitcase,
        SuitcaseMedical,
        SuitcaseRolling,
        Sun,
        SunPlantWilt,
        Superscript,
        Swatchbook,
        Synagogue,
        Syringe,
        T,
        Table,
        TableCells,
        TableCellsLarge,
        TableColumns,
        TableList,
        TableTennisPaddleBall,
        Tablet,
        TabletButton,
        TabletScreenButton,
        Tablets,
        TachographDigital,
        Tag,
        Tags,
        Tape,
        Tarp,
        TarpDroplet,
        Taxi,
        Teeth,
        TeethOpen,
        TemperatureArrowDown,
        TemperatureArrowUp,
        TemperatureEmpty,
        TemperatureFull,
        TemperatureHalf,
        TemperatureHigh,
        TemperatureLow,
        TemperatureQuarter,
        TemperatureThreeQuarters,
        TengeSign,
        Tent,
        TentArrowDownToLine,
        TentArrowLeftRight,
        TentArrowTurnLeft,
        TentArrowsDown,
        Tents,
        Terminal,
        TextHeight,
        TextSlash,
        TextWidth,
        Thermometer,
        ThumbsDown,
        ThumbsUp,
        Thumbtack,
        Ticket,
        TicketSimple,
        Timeline,
        ToggleOff,
        ToggleOn,
        Toilet,
        ToiletPaper,
        ToiletPaperSlash,
        ToiletPortable,
        ToiletsPortable,
        Toolbox,
        Tooth,
        ToriiGate,
        Tornado,
        TowerBroadcast,
        TowerCell,
        TowerObservation,
        Tractor,
        Trademark,
        TrafficLight,
        Trailer,
        Train,
        TrainSubway,
        TrainTram,
        Transgender,
        Trash,
        TrashArrowUp,
        TrashCan,
        TrashCanArrowUp,
        Tree,
        TreeCity,
        TriangleExclamation,
        Trophy,
        Trowel,
        TrowelBricks,
        Truck,
        TruckArrowRight,
        TruckDroplet,
        TruckFast,
        TruckField,
        TruckFieldUn,
        TruckFront,
        TruckMedical,
        TruckMonster,
        TruckMoving,
        TruckPickup,
        TruckPlane,
        TruckRampBox,
        Tty,
        TurkishLiraSign,
        TurnDown,
        TurnUp,
        Tv,
        U,
        Umbrella,
        UmbrellaBeach,
        Underline,
        UniversalAccess,
        Unlock,
        UnlockKeyhole,
        UpDown,
        UpDownLeftRight,
        UpLong,
        UpRightAndDownLeftFromCenter,
        UpRightFromSquare,
        Upload,
        User,
        UserAstronaut,
        UserCheck,
        UserClock,
        UserDoctor,
        UserGear,
        UserGraduate,
        UserGroup,
        UserInjured,
        UserLarge,
        UserLargeSlash,
        UserLock,
        UserMinus,
        UserNinja,
        UserNurse,
        UserPen,
        UserPlus,
        UserSecret,
        UserShield,
        UserSlash,
        UserTag,
        UserTie,
        UserXmark,
        Users,
        UsersBetweenLines,
        UsersGear,
        UsersLine,
        UsersRays,
        UsersRectangle,
        UsersSlash,
        UsersViewfinder,
        Utensils,
        V,
        VanShuttle,
        Vault,
        VectorSquare,
        Venus,
        VenusDouble,
        VenusMars,
        Vest,
        VestPatches,
        Vial,
        VialCircleCheck,
        VialVirus,
        Vials,
        Video,
        VideoSlash,
        Vihara,
        Virus,
        VirusCovid,
        VirusCovidSlash,
        VirusSlash,
        Viruses,
        Voicemail,
        Volcano,
        Volleyball,
        VolumeHigh,
        VolumeLow,
        VolumeOff,
        VolumeXmark,
        VrCardboard,
        W,
        WalkieTalkie,
        Wallet,
        WandMagic,
        WandMagicSparkles,
        WandSparkles,
        Warehouse,
        Water,
        WaterLadder,
        WaveSquare,
        WeightHanging,
        WeightScale,
        WheatAwn,
        WheatAwnCircleExclamation,
        Wheelchair,
        WheelchairMove,
        WhiskeyGlass,
        Wifi,
        Wind,
        WindowMaximize,
        WindowMinimize,
        WindowRestore,
        WineBottle,
        WineGlass,
        WineGlassEmpty,
        WonSign,
        Worm,
        Wrench,
        X,
        XRay,
        Xmark,
        XmarksLines,
        Y,
        YenSign,
        YinYang,
        Z,
    }



    internal sealed class FontInstance : IDisposable
    {
        private readonly Dictionary<FontSize, Font> _InstanceBySize = new Dictionary<FontSize, Font>(8);

        public FontInstance(FontFamily family, FontStyle style, float sizeChange = 1.0f)
        {
            _InstanceBySize.Add( FontSize.Tiny,     new Font( family, (float)Math.Round(  8.0f + sizeChange), style ) );
            _InstanceBySize.Add( FontSize.Smaller,  new Font( family, (float)Math.Round( 11.0f + sizeChange), style ) );
            _InstanceBySize.Add( FontSize.Normal,   new Font( family, (float)Math.Round( 14.0f + sizeChange), style ) );
            _InstanceBySize.Add( FontSize.Larger,   new Font( family, (float)Math.Round( 20.0f + sizeChange), style ) );
            _InstanceBySize.Add( FontSize.Title,    new Font( family, (float)Math.Round( 28.0f + sizeChange), style ) );
        }

        public void Dispose()
        {
            foreach ( Font f in _InstanceBySize.Values )
                f.Dispose();

            _InstanceBySize.Clear();
        }

        public Font BySize( FontSize sz )
        {
            return _InstanceBySize[sz];
        }
    }


    public sealed class FontLibrary : IDisposable
    {
        private static readonly Lazy<FontLibrary> _Lazy = new Lazy<FontLibrary>(() => new FontLibrary());
        public static FontLibrary Instance => _Lazy.Value;

        public bool IsLoaded()
        {
            return _FontDataIcon != null && _FontDataByWeight.Count > 0;
        }

        public Font Get( FontChoice w, FontSize s )
        {
            return _FontDataByWeight[w].BySize( s );
        }

        public Font GetIcons( FontSize s )
        {
            return _FontDataIcon.BySize( s );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        private static int IconIDToUtf( IconID iid )
        {
            switch ( iid )
            {
                case IconID.Num0:                              return 0x0030;
                case IconID.Num1:                              return 0x0031;
                case IconID.Num2:                              return 0x0032;
                case IconID.Num3:                              return 0x0033;
                case IconID.Num4:                              return 0x0034;
                case IconID.Num5:                              return 0x0035;
                case IconID.Num6:                              return 0x0036;
                case IconID.Num7:                              return 0x0037;
                case IconID.Num8:                              return 0x0038;
                case IconID.Num9:                              return 0x0039;
                case IconID.A:                                 return 0x0041;
                case IconID.AddressBook:                       return 0xf2b9;
                case IconID.AddressCard:                       return 0xf2bb;
                case IconID.AlignCenter:                       return 0xf037;
                case IconID.AlignJustify:                      return 0xf039;
                case IconID.AlignLeft:                         return 0xf036;
                case IconID.AlignRight:                        return 0xf038;
                case IconID.Anchor:                            return 0xf13d;
                case IconID.AnchorCircleCheck:                 return 0xe4aa;
                case IconID.AnchorCircleExclamation:           return 0xe4ab;
                case IconID.AnchorCircleXmark:                 return 0xe4ac;
                case IconID.AnchorLock:                        return 0xe4ad;
                case IconID.AngleDown:                         return 0xf107;
                case IconID.AngleLeft:                         return 0xf104;
                case IconID.AngleRight:                        return 0xf105;
                case IconID.AngleUp:                           return 0xf106;
                case IconID.AnglesDown:                        return 0xf103;
                case IconID.AnglesLeft:                        return 0xf100;
                case IconID.AnglesRight:                       return 0xf101;
                case IconID.AnglesUp:                          return 0xf102;
                case IconID.Ankh:                              return 0xf644;
                case IconID.AppleWhole:                        return 0xf5d1;
                case IconID.Archway:                           return 0xf557;
                case IconID.ArrowDown:                         return 0xf063;
                case IconID.ArrowDown19:                       return 0xf162;
                case IconID.ArrowDown91:                       return 0xf886;
                case IconID.ArrowDownAZ:                       return 0xf15d;
                case IconID.ArrowDownLong:                     return 0xf175;
                case IconID.ArrowDownShortWide:                return 0xf884;
                case IconID.ArrowDownUpAcrossLine:             return 0xe4af;
                case IconID.ArrowDownUpLock:                   return 0xe4b0;
                case IconID.ArrowDownWideShort:                return 0xf160;
                case IconID.ArrowDownZA:                       return 0xf881;
                case IconID.ArrowLeft:                         return 0xf060;
                case IconID.ArrowLeftLong:                     return 0xf177;
                case IconID.ArrowPointer:                      return 0xf245;
                case IconID.ArrowRight:                        return 0xf061;
                case IconID.ArrowRightArrowLeft:               return 0xf0ec;
                case IconID.ArrowRightFromBracket:             return 0xf08b;
                case IconID.ArrowRightLong:                    return 0xf178;
                case IconID.ArrowRightToBracket:               return 0xf090;
                case IconID.ArrowRightToCity:                  return 0xe4b3;
                case IconID.ArrowRotateLeft:                   return 0xf0e2;
                case IconID.ArrowRotateRight:                  return 0xf01e;
                case IconID.ArrowTrendDown:                    return 0xe097;
                case IconID.ArrowTrendUp:                      return 0xe098;
                case IconID.ArrowTurnDown:                     return 0xf149;
                case IconID.ArrowTurnUp:                       return 0xf148;
                case IconID.ArrowUp:                           return 0xf062;
                case IconID.ArrowUp19:                         return 0xf163;
                case IconID.ArrowUp91:                         return 0xf887;
                case IconID.ArrowUpAZ:                         return 0xf15e;
                case IconID.ArrowUpFromBracket:                return 0xe09a;
                case IconID.ArrowUpFromGroundWater:            return 0xe4b5;
                case IconID.ArrowUpFromWaterPump:              return 0xe4b6;
                case IconID.ArrowUpLong:                       return 0xf176;
                case IconID.ArrowUpRightDots:                  return 0xe4b7;
                case IconID.ArrowUpRightFromSquare:            return 0xf08e;
                case IconID.ArrowUpShortWide:                  return 0xf885;
                case IconID.ArrowUpWideShort:                  return 0xf161;
                case IconID.ArrowUpZA:                         return 0xf882;
                case IconID.ArrowsDownToLine:                  return 0xe4b8;
                case IconID.ArrowsDownToPeople:                return 0xe4b9;
                case IconID.ArrowsLeftRight:                   return 0xf07e;
                case IconID.ArrowsLeftRightToLine:             return 0xe4ba;
                case IconID.ArrowsRotate:                      return 0xf021;
                case IconID.ArrowsSpin:                        return 0xe4bb;
                case IconID.ArrowsSplitUpAndLeft:              return 0xe4bc;
                case IconID.ArrowsToCircle:                    return 0xe4bd;
                case IconID.ArrowsToDot:                       return 0xe4be;
                case IconID.ArrowsToEye:                       return 0xe4bf;
                case IconID.ArrowsTurnRight:                   return 0xe4c0;
                case IconID.ArrowsTurnToDots:                  return 0xe4c1;
                case IconID.ArrowsUpDown:                      return 0xf07d;
                case IconID.ArrowsUpDownLeftRight:             return 0xf047;
                case IconID.ArrowsUpToLine:                    return 0xe4c2;
                case IconID.Asterisk:                          return 0x002a;
                case IconID.At:                                return 0x0040;
                case IconID.Atom:                              return 0xf5d2;
                case IconID.AudioDescription:                  return 0xf29e;
                case IconID.AustralSign:                       return 0xe0a9;
                case IconID.Award:                             return 0xf559;
                case IconID.B:                                 return 0x0042;
                case IconID.Baby:                              return 0xf77c;
                case IconID.BabyCarriage:                      return 0xf77d;
                case IconID.Backward:                          return 0xf04a;
                case IconID.BackwardFast:                      return 0xf049;
                case IconID.BackwardStep:                      return 0xf048;
                case IconID.Bacon:                             return 0xf7e5;
                case IconID.Bacteria:                          return 0xe059;
                case IconID.Bacterium:                         return 0xe05a;
                case IconID.BagShopping:                       return 0xf290;
                case IconID.Bahai:                             return 0xf666;
                case IconID.BahtSign:                          return 0xe0ac;
                case IconID.Ban:                               return 0xf05e;
                case IconID.BanSmoking:                        return 0xf54d;
                case IconID.Bandage:                           return 0xf462;
                case IconID.BangladeshiTakaSign:               return 0xe2e6;
                case IconID.Barcode:                           return 0xf02a;
                case IconID.Bars:                              return 0xf0c9;
                case IconID.BarsProgress:                      return 0xf828;
                case IconID.BarsStaggered:                     return 0xf550;
                case IconID.Baseball:                          return 0xf433;
                case IconID.BaseballBatBall:                   return 0xf432;
                case IconID.BasketShopping:                    return 0xf291;
                case IconID.Basketball:                        return 0xf434;
                case IconID.Bath:                              return 0xf2cd;
                case IconID.BatteryEmpty:                      return 0xf244;
                case IconID.BatteryFull:                       return 0xf240;
                case IconID.BatteryHalf:                       return 0xf242;
                case IconID.BatteryQuarter:                    return 0xf243;
                case IconID.BatteryThreeQuarters:              return 0xf241;
                case IconID.Bed:                               return 0xf236;
                case IconID.BedPulse:                          return 0xf487;
                case IconID.BeerMugEmpty:                      return 0xf0fc;
                case IconID.Bell:                              return 0xf0f3;
                case IconID.BellConcierge:                     return 0xf562;
                case IconID.BellSlash:                         return 0xf1f6;
                case IconID.BezierCurve:                       return 0xf55b;
                case IconID.Bicycle:                           return 0xf206;
                case IconID.Binoculars:                        return 0xf1e5;
                case IconID.Biohazard:                         return 0xf780;
                case IconID.BitcoinSign:                       return 0xe0b4;
                case IconID.Blender:                           return 0xf517;
                case IconID.BlenderPhone:                      return 0xf6b6;
                case IconID.Blog:                              return 0xf781;
                case IconID.Bold:                              return 0xf032;
                case IconID.Bolt:                              return 0xf0e7;
                case IconID.BoltLightning:                     return 0xe0b7;
                case IconID.Bomb:                              return 0xf1e2;
                case IconID.Bone:                              return 0xf5d7;
                case IconID.Bong:                              return 0xf55c;
                case IconID.Book:                              return 0xf02d;
                case IconID.BookAtlas:                         return 0xf558;
                case IconID.BookBible:                         return 0xf647;
                case IconID.BookBookmark:                      return 0xe0bb;
                case IconID.BookJournalWhills:                 return 0xf66a;
                case IconID.BookMedical:                       return 0xf7e6;
                case IconID.BookOpen:                          return 0xf518;
                case IconID.BookOpenReader:                    return 0xf5da;
                case IconID.BookQuran:                         return 0xf687;
                case IconID.BookSkull:                         return 0xf6b7;
                case IconID.BookTanakh:                        return 0xf827;
                case IconID.Bookmark:                          return 0xf02e;
                case IconID.BorderAll:                         return 0xf84c;
                case IconID.BorderNone:                        return 0xf850;
                case IconID.BorderTopLeft:                     return 0xf853;
                case IconID.BoreHole:                          return 0xe4c3;
                case IconID.BottleDroplet:                     return 0xe4c4;
                case IconID.BottleWater:                       return 0xe4c5;
                case IconID.BowlFood:                          return 0xe4c6;
                case IconID.BowlRice:                          return 0xe2eb;
                case IconID.BowlingBall:                       return 0xf436;
                case IconID.Box:                               return 0xf466;
                case IconID.BoxArchive:                        return 0xf187;
                case IconID.BoxOpen:                           return 0xf49e;
                case IconID.BoxTissue:                         return 0xe05b;
                case IconID.BoxesPacking:                      return 0xe4c7;
                case IconID.BoxesStacked:                      return 0xf468;
                case IconID.Braille:                           return 0xf2a1;
                case IconID.Brain:                             return 0xf5dc;
                case IconID.BrazilianRealSign:                 return 0xe46c;
                case IconID.BreadSlice:                        return 0xf7ec;
                case IconID.Bridge:                            return 0xe4c8;
                case IconID.BridgeCircleCheck:                 return 0xe4c9;
                case IconID.BridgeCircleExclamation:           return 0xe4ca;
                case IconID.BridgeCircleXmark:                 return 0xe4cb;
                case IconID.BridgeLock:                        return 0xe4cc;
                case IconID.BridgeWater:                       return 0xe4ce;
                case IconID.Briefcase:                         return 0xf0b1;
                case IconID.BriefcaseMedical:                  return 0xf469;
                case IconID.Broom:                             return 0xf51a;
                case IconID.BroomBall:                         return 0xf458;
                case IconID.Brush:                             return 0xf55d;
                case IconID.Bucket:                            return 0xe4cf;
                case IconID.Bug:                               return 0xf188;
                case IconID.BugSlash:                          return 0xe490;
                case IconID.Bugs:                              return 0xe4d0;
                case IconID.Building:                          return 0xf1ad;
                case IconID.BuildingCircleArrowRight:          return 0xe4d1;
                case IconID.BuildingCircleCheck:               return 0xe4d2;
                case IconID.BuildingCircleExclamation:         return 0xe4d3;
                case IconID.BuildingCircleXmark:               return 0xe4d4;
                case IconID.BuildingColumns:                   return 0xf19c;
                case IconID.BuildingFlag:                      return 0xe4d5;
                case IconID.BuildingLock:                      return 0xe4d6;
                case IconID.BuildingNgo:                       return 0xe4d7;
                case IconID.BuildingShield:                    return 0xe4d8;
                case IconID.BuildingUn:                        return 0xe4d9;
                case IconID.BuildingUser:                      return 0xe4da;
                case IconID.BuildingWheat:                     return 0xe4db;
                case IconID.Bullhorn:                          return 0xf0a1;
                case IconID.Bullseye:                          return 0xf140;
                case IconID.Burger:                            return 0xf805;
                case IconID.Burst:                             return 0xe4dc;
                case IconID.Bus:                               return 0xf207;
                case IconID.BusSimple:                         return 0xf55e;
                case IconID.BusinessTime:                      return 0xf64a;
                case IconID.C:                                 return 0x0043;
                case IconID.CableCar:                          return 0xf7da;
                case IconID.CakeCandles:                       return 0xf1fd;
                case IconID.Calculator:                        return 0xf1ec;
                case IconID.Calendar:                          return 0xf133;
                case IconID.CalendarCheck:                     return 0xf274;
                case IconID.CalendarDay:                       return 0xf783;
                case IconID.CalendarDays:                      return 0xf073;
                case IconID.CalendarMinus:                     return 0xf272;
                case IconID.CalendarPlus:                      return 0xf271;
                case IconID.CalendarWeek:                      return 0xf784;
                case IconID.CalendarXmark:                     return 0xf273;
                case IconID.Camera:                            return 0xf030;
                case IconID.CameraRetro:                       return 0xf083;
                case IconID.CameraRotate:                      return 0xe0d8;
                case IconID.Campground:                        return 0xf6bb;
                case IconID.CandyCane:                         return 0xf786;
                case IconID.Cannabis:                          return 0xf55f;
                case IconID.Capsules:                          return 0xf46b;
                case IconID.Car:                               return 0xf1b9;
                case IconID.CarBattery:                        return 0xf5df;
                case IconID.CarBurst:                          return 0xf5e1;
                case IconID.CarOn:                             return 0xe4dd;
                case IconID.CarRear:                           return 0xf5de;
                case IconID.CarSide:                           return 0xf5e4;
                case IconID.CarTunnel:                         return 0xe4de;
                case IconID.Caravan:                           return 0xf8ff;
                case IconID.CaretDown:                         return 0xf0d7;
                case IconID.CaretLeft:                         return 0xf0d9;
                case IconID.CaretRight:                        return 0xf0da;
                case IconID.CaretUp:                           return 0xf0d8;
                case IconID.Carrot:                            return 0xf787;
                case IconID.CartArrowDown:                     return 0xf218;
                case IconID.CartFlatbed:                       return 0xf474;
                case IconID.CartFlatbedSuitcase:               return 0xf59d;
                case IconID.CartPlus:                          return 0xf217;
                case IconID.CartShopping:                      return 0xf07a;
                case IconID.CashRegister:                      return 0xf788;
                case IconID.Cat:                               return 0xf6be;
                case IconID.CediSign:                          return 0xe0df;
                case IconID.CentSign:                          return 0xe3f5;
                case IconID.Certificate:                       return 0xf0a3;
                case IconID.Chair:                             return 0xf6c0;
                case IconID.Chalkboard:                        return 0xf51b;
                case IconID.ChalkboardUser:                    return 0xf51c;
                case IconID.ChampagneGlasses:                  return 0xf79f;
                case IconID.ChargingStation:                   return 0xf5e7;
                case IconID.ChartArea:                         return 0xf1fe;
                case IconID.ChartBar:                          return 0xf080;
                case IconID.ChartColumn:                       return 0xe0e3;
                case IconID.ChartGantt:                        return 0xe0e4;
                case IconID.ChartLine:                         return 0xf201;
                case IconID.ChartPie:                          return 0xf200;
                case IconID.ChartSimple:                       return 0xe473;
                case IconID.Check:                             return 0xf00c;
                case IconID.CheckDouble:                       return 0xf560;
                case IconID.CheckToSlot:                       return 0xf772;
                case IconID.Cheese:                            return 0xf7ef;
                case IconID.Chess:                             return 0xf439;
                case IconID.ChessBishop:                       return 0xf43a;
                case IconID.ChessBoard:                        return 0xf43c;
                case IconID.ChessKing:                         return 0xf43f;
                case IconID.ChessKnight:                       return 0xf441;
                case IconID.ChessPawn:                         return 0xf443;
                case IconID.ChessQueen:                        return 0xf445;
                case IconID.ChessRook:                         return 0xf447;
                case IconID.ChevronDown:                       return 0xf078;
                case IconID.ChevronLeft:                       return 0xf053;
                case IconID.ChevronRight:                      return 0xf054;
                case IconID.ChevronUp:                         return 0xf077;
                case IconID.Child:                             return 0xf1ae;
                case IconID.ChildCombatant:                    return 0xe4e0;
                case IconID.ChildDress:                        return 0xe59c;
                case IconID.ChildReaching:                     return 0xe59d;
                case IconID.Children:                          return 0xe4e1;
                case IconID.Church:                            return 0xf51d;
                case IconID.Circle:                            return 0xf111;
                case IconID.CircleArrowDown:                   return 0xf0ab;
                case IconID.CircleArrowLeft:                   return 0xf0a8;
                case IconID.CircleArrowRight:                  return 0xf0a9;
                case IconID.CircleArrowUp:                     return 0xf0aa;
                case IconID.CircleCheck:                       return 0xf058;
                case IconID.CircleChevronDown:                 return 0xf13a;
                case IconID.CircleChevronLeft:                 return 0xf137;
                case IconID.CircleChevronRight:                return 0xf138;
                case IconID.CircleChevronUp:                   return 0xf139;
                case IconID.CircleDollarToSlot:                return 0xf4b9;
                case IconID.CircleDot:                         return 0xf192;
                case IconID.CircleDown:                        return 0xf358;
                case IconID.CircleExclamation:                 return 0xf06a;
                case IconID.CircleH:                           return 0xf47e;
                case IconID.CircleHalfStroke:                  return 0xf042;
                case IconID.CircleInfo:                        return 0xf05a;
                case IconID.CircleLeft:                        return 0xf359;
                case IconID.CircleMinus:                       return 0xf056;
                case IconID.CircleNodes:                       return 0xe4e2;
                case IconID.CircleNotch:                       return 0xf1ce;
                case IconID.CirclePause:                       return 0xf28b;
                case IconID.CirclePlay:                        return 0xf144;
                case IconID.CirclePlus:                        return 0xf055;
                case IconID.CircleQuestion:                    return 0xf059;
                case IconID.CircleRadiation:                   return 0xf7ba;
                case IconID.CircleRight:                       return 0xf35a;
                case IconID.CircleStop:                        return 0xf28d;
                case IconID.CircleUp:                          return 0xf35b;
                case IconID.CircleUser:                        return 0xf2bd;
                case IconID.CircleXmark:                       return 0xf057;
                case IconID.City:                              return 0xf64f;
                case IconID.Clapperboard:                      return 0xe131;
                case IconID.Clipboard:                         return 0xf328;
                case IconID.ClipboardCheck:                    return 0xf46c;
                case IconID.ClipboardList:                     return 0xf46d;
                case IconID.ClipboardQuestion:                 return 0xe4e3;
                case IconID.ClipboardUser:                     return 0xf7f3;
                case IconID.Clock:                             return 0xf017;
                case IconID.ClockRotateLeft:                   return 0xf1da;
                case IconID.Clone:                             return 0xf24d;
                case IconID.ClosedCaptioning:                  return 0xf20a;
                case IconID.Cloud:                             return 0xf0c2;
                case IconID.CloudArrowDown:                    return 0xf0ed;
                case IconID.CloudArrowUp:                      return 0xf0ee;
                case IconID.CloudBolt:                         return 0xf76c;
                case IconID.CloudMeatball:                     return 0xf73b;
                case IconID.CloudMoon:                         return 0xf6c3;
                case IconID.CloudMoonRain:                     return 0xf73c;
                case IconID.CloudRain:                         return 0xf73d;
                case IconID.CloudShowersHeavy:                 return 0xf740;
                case IconID.CloudShowersWater:                 return 0xe4e4;
                case IconID.CloudSun:                          return 0xf6c4;
                case IconID.CloudSunRain:                      return 0xf743;
                case IconID.Clover:                            return 0xe139;
                case IconID.Code:                              return 0xf121;
                case IconID.CodeBranch:                        return 0xf126;
                case IconID.CodeCommit:                        return 0xf386;
                case IconID.CodeCompare:                       return 0xe13a;
                case IconID.CodeFork:                          return 0xe13b;
                case IconID.CodeMerge:                         return 0xf387;
                case IconID.CodePullRequest:                   return 0xe13c;
                case IconID.Coins:                             return 0xf51e;
                case IconID.ColonSign:                         return 0xe140;
                case IconID.Comment:                           return 0xf075;
                case IconID.CommentDollar:                     return 0xf651;
                case IconID.CommentDots:                       return 0xf4ad;
                case IconID.CommentMedical:                    return 0xf7f5;
                case IconID.CommentSlash:                      return 0xf4b3;
                case IconID.CommentSms:                        return 0xf7cd;
                case IconID.Comments:                          return 0xf086;
                case IconID.CommentsDollar:                    return 0xf653;
                case IconID.CompactDisc:                       return 0xf51f;
                case IconID.Compass:                           return 0xf14e;
                case IconID.CompassDrafting:                   return 0xf568;
                case IconID.Compress:                          return 0xf066;
                case IconID.Computer:                          return 0xe4e5;
                case IconID.ComputerMouse:                     return 0xf8cc;
                case IconID.Cookie:                            return 0xf563;
                case IconID.CookieBite:                        return 0xf564;
                case IconID.Copy:                              return 0xf0c5;
                case IconID.Copyright:                         return 0xf1f9;
                case IconID.Couch:                             return 0xf4b8;
                case IconID.Cow:                               return 0xf6c8;
                case IconID.CreditCard:                        return 0xf09d;
                case IconID.Crop:                              return 0xf125;
                case IconID.CropSimple:                        return 0xf565;
                case IconID.Cross:                             return 0xf654;
                case IconID.Crosshairs:                        return 0xf05b;
                case IconID.Crow:                              return 0xf520;
                case IconID.Crown:                             return 0xf521;
                case IconID.Crutch:                            return 0xf7f7;
                case IconID.CruzeiroSign:                      return 0xe152;
                case IconID.Cube:                              return 0xf1b2;
                case IconID.Cubes:                             return 0xf1b3;
                case IconID.CubesStacked:                      return 0xe4e6;
                case IconID.D:                                 return 0x0044;
                case IconID.Database:                          return 0xf1c0;
                case IconID.DeleteLeft:                        return 0xf55a;
                case IconID.Democrat:                          return 0xf747;
                case IconID.Desktop:                           return 0xf390;
                case IconID.Dharmachakra:                      return 0xf655;
                case IconID.DiagramNext:                       return 0xe476;
                case IconID.DiagramPredecessor:                return 0xe477;
                case IconID.DiagramProject:                    return 0xf542;
                case IconID.DiagramSuccessor:                  return 0xe47a;
                case IconID.Diamond:                           return 0xf219;
                case IconID.DiamondTurnRight:                  return 0xf5eb;
                case IconID.Dice:                              return 0xf522;
                case IconID.DiceD20:                           return 0xf6cf;
                case IconID.DiceD6:                            return 0xf6d1;
                case IconID.DiceFive:                          return 0xf523;
                case IconID.DiceFour:                          return 0xf524;
                case IconID.DiceOne:                           return 0xf525;
                case IconID.DiceSix:                           return 0xf526;
                case IconID.DiceThree:                         return 0xf527;
                case IconID.DiceTwo:                           return 0xf528;
                case IconID.Disease:                           return 0xf7fa;
                case IconID.Display:                           return 0xe163;
                case IconID.Divide:                            return 0xf529;
                case IconID.Dna:                               return 0xf471;
                case IconID.Dog:                               return 0xf6d3;
                case IconID.DollarSign:                        return 0x0024;
                case IconID.Dolly:                             return 0xf472;
                case IconID.DongSign:                          return 0xe169;
                case IconID.DoorClosed:                        return 0xf52a;
                case IconID.DoorOpen:                          return 0xf52b;
                case IconID.Dove:                              return 0xf4ba;
                case IconID.DownLeftAndUpRightToCenter:        return 0xf422;
                case IconID.DownLong:                          return 0xf309;
                case IconID.Download:                          return 0xf019;
                case IconID.Dragon:                            return 0xf6d5;
                case IconID.DrawPolygon:                       return 0xf5ee;
                case IconID.Droplet:                           return 0xf043;
                case IconID.DropletSlash:                      return 0xf5c7;
                case IconID.Drum:                              return 0xf569;
                case IconID.DrumSteelpan:                      return 0xf56a;
                case IconID.DrumstickBite:                     return 0xf6d7;
                case IconID.Dumbbell:                          return 0xf44b;
                case IconID.Dumpster:                          return 0xf793;
                case IconID.DumpsterFire:                      return 0xf794;
                case IconID.Dungeon:                           return 0xf6d9;
                case IconID.E:                                 return 0x0045;
                case IconID.EarDeaf:                           return 0xf2a4;
                case IconID.EarListen:                         return 0xf2a2;
                case IconID.EarthAfrica:                       return 0xf57c;
                case IconID.EarthAmericas:                     return 0xf57d;
                case IconID.EarthAsia:                         return 0xf57e;
                case IconID.EarthEurope:                       return 0xf7a2;
                case IconID.EarthOceania:                      return 0xe47b;
                case IconID.Egg:                               return 0xf7fb;
                case IconID.Eject:                             return 0xf052;
                case IconID.Elevator:                          return 0xe16d;
                case IconID.Ellipsis:                          return 0xf141;
                case IconID.EllipsisVertical:                  return 0xf142;
                case IconID.Envelope:                          return 0xf0e0;
                case IconID.EnvelopeCircleCheck:               return 0xe4e8;
                case IconID.EnvelopeOpen:                      return 0xf2b6;
                case IconID.EnvelopeOpenText:                  return 0xf658;
                case IconID.EnvelopesBulk:                     return 0xf674;
                case IconID.Equals:                            return 0x003d;
                case IconID.Eraser:                            return 0xf12d;
                case IconID.Ethernet:                          return 0xf796;
                case IconID.EuroSign:                          return 0xf153;
                case IconID.Exclamation:                       return 0x0021;
                case IconID.Expand:                            return 0xf065;
                case IconID.Explosion:                         return 0xe4e9;
                case IconID.Eye:                               return 0xf06e;
                case IconID.EyeDropper:                        return 0xf1fb;
                case IconID.EyeLowVision:                      return 0xf2a8;
                case IconID.EyeSlash:                          return 0xf070;
                case IconID.F:                                 return 0x0046;
                case IconID.FaceAngry:                         return 0xf556;
                case IconID.FaceDizzy:                         return 0xf567;
                case IconID.FaceFlushed:                       return 0xf579;
                case IconID.FaceFrown:                         return 0xf119;
                case IconID.FaceFrownOpen:                     return 0xf57a;
                case IconID.FaceGrimace:                       return 0xf57f;
                case IconID.FaceGrin:                          return 0xf580;
                case IconID.FaceGrinBeam:                      return 0xf582;
                case IconID.FaceGrinBeamSweat:                 return 0xf583;
                case IconID.FaceGrinHearts:                    return 0xf584;
                case IconID.FaceGrinSquint:                    return 0xf585;
                case IconID.FaceGrinSquintTears:               return 0xf586;
                case IconID.FaceGrinStars:                     return 0xf587;
                case IconID.FaceGrinTears:                     return 0xf588;
                case IconID.FaceGrinTongue:                    return 0xf589;
                case IconID.FaceGrinTongueSquint:              return 0xf58a;
                case IconID.FaceGrinTongueWink:                return 0xf58b;
                case IconID.FaceGrinWide:                      return 0xf581;
                case IconID.FaceGrinWink:                      return 0xf58c;
                case IconID.FaceKiss:                          return 0xf596;
                case IconID.FaceKissBeam:                      return 0xf597;
                case IconID.FaceKissWinkHeart:                 return 0xf598;
                case IconID.FaceLaugh:                         return 0xf599;
                case IconID.FaceLaughBeam:                     return 0xf59a;
                case IconID.FaceLaughSquint:                   return 0xf59b;
                case IconID.FaceLaughWink:                     return 0xf59c;
                case IconID.FaceMeh:                           return 0xf11a;
                case IconID.FaceMehBlank:                      return 0xf5a4;
                case IconID.FaceRollingEyes:                   return 0xf5a5;
                case IconID.FaceSadCry:                        return 0xf5b3;
                case IconID.FaceSadTear:                       return 0xf5b4;
                case IconID.FaceSmile:                         return 0xf118;
                case IconID.FaceSmileBeam:                     return 0xf5b8;
                case IconID.FaceSmileWink:                     return 0xf4da;
                case IconID.FaceSurprise:                      return 0xf5c2;
                case IconID.FaceTired:                         return 0xf5c8;
                case IconID.Fan:                               return 0xf863;
                case IconID.Faucet:                            return 0xe005;
                case IconID.FaucetDrip:                        return 0xe006;
                case IconID.Fax:                               return 0xf1ac;
                case IconID.Feather:                           return 0xf52d;
                case IconID.FeatherPointed:                    return 0xf56b;
                case IconID.Ferry:                             return 0xe4ea;
                case IconID.File:                              return 0xf15b;
                case IconID.FileArrowDown:                     return 0xf56d;
                case IconID.FileArrowUp:                       return 0xf574;
                case IconID.FileAudio:                         return 0xf1c7;
                case IconID.FileCircleCheck:                   return 0xe5a0;
                case IconID.FileCircleExclamation:             return 0xe4eb;
                case IconID.FileCircleMinus:                   return 0xe4ed;
                case IconID.FileCirclePlus:                    return 0xe494;
                case IconID.FileCircleQuestion:                return 0xe4ef;
                case IconID.FileCircleXmark:                   return 0xe5a1;
                case IconID.FileCode:                          return 0xf1c9;
                case IconID.FileContract:                      return 0xf56c;
                case IconID.FileCsv:                           return 0xf6dd;
                case IconID.FileExcel:                         return 0xf1c3;
                case IconID.FileExport:                        return 0xf56e;
                case IconID.FileImage:                         return 0xf1c5;
                case IconID.FileImport:                        return 0xf56f;
                case IconID.FileInvoice:                       return 0xf570;
                case IconID.FileInvoiceDollar:                 return 0xf571;
                case IconID.FileLines:                         return 0xf15c;
                case IconID.FileMedical:                       return 0xf477;
                case IconID.FilePdf:                           return 0xf1c1;
                case IconID.FilePen:                           return 0xf31c;
                case IconID.FilePowerpoint:                    return 0xf1c4;
                case IconID.FilePrescription:                  return 0xf572;
                case IconID.FileShield:                        return 0xe4f0;
                case IconID.FileSignature:                     return 0xf573;
                case IconID.FileVideo:                         return 0xf1c8;
                case IconID.FileWaveform:                      return 0xf478;
                case IconID.FileWord:                          return 0xf1c2;
                case IconID.FileZipper:                        return 0xf1c6;
                case IconID.Fill:                              return 0xf575;
                case IconID.FillDrip:                          return 0xf576;
                case IconID.Film:                              return 0xf008;
                case IconID.Filter:                            return 0xf0b0;
                case IconID.FilterCircleDollar:                return 0xf662;
                case IconID.FilterCircleXmark:                 return 0xe17b;
                case IconID.Fingerprint:                       return 0xf577;
                case IconID.Fire:                              return 0xf06d;
                case IconID.FireBurner:                        return 0xe4f1;
                case IconID.FireExtinguisher:                  return 0xf134;
                case IconID.FireFlameCurved:                   return 0xf7e4;
                case IconID.FireFlameSimple:                   return 0xf46a;
                case IconID.Fish:                              return 0xf578;
                case IconID.FishFins:                          return 0xe4f2;
                case IconID.Flag:                              return 0xf024;
                case IconID.FlagCheckered:                     return 0xf11e;
                case IconID.FlagUsa:                           return 0xf74d;
                case IconID.Flask:                             return 0xf0c3;
                case IconID.FlaskVial:                         return 0xe4f3;
                case IconID.FloppyDisk:                        return 0xf0c7;
                case IconID.FlorinSign:                        return 0xe184;
                case IconID.Folder:                            return 0xf07b;
                case IconID.FolderClosed:                      return 0xe185;
                case IconID.FolderMinus:                       return 0xf65d;
                case IconID.FolderOpen:                        return 0xf07c;
                case IconID.FolderPlus:                        return 0xf65e;
                case IconID.FolderTree:                        return 0xf802;
                case IconID.Font:                              return 0xf031;
                case IconID.FontAwesome:                       return 0xf2b4;
                case IconID.Football:                          return 0xf44e;
                case IconID.Forward:                           return 0xf04e;
                case IconID.ForwardFast:                       return 0xf050;
                case IconID.ForwardStep:                       return 0xf051;
                case IconID.FrancSign:                         return 0xe18f;
                case IconID.Frog:                              return 0xf52e;
                case IconID.Futbol:                            return 0xf1e3;
                case IconID.G:                                 return 0x0047;
                case IconID.Gamepad:                           return 0xf11b;
                case IconID.GasPump:                           return 0xf52f;
                case IconID.Gauge:                             return 0xf624;
                case IconID.GaugeHigh:                         return 0xf625;
                case IconID.GaugeSimple:                       return 0xf629;
                case IconID.GaugeSimpleHigh:                   return 0xf62a;
                case IconID.Gavel:                             return 0xf0e3;
                case IconID.Gear:                              return 0xf013;
                case IconID.Gears:                             return 0xf085;
                case IconID.Gem:                               return 0xf3a5;
                case IconID.Genderless:                        return 0xf22d;
                case IconID.Ghost:                             return 0xf6e2;
                case IconID.Gift:                              return 0xf06b;
                case IconID.Gifts:                             return 0xf79c;
                case IconID.GlassWater:                        return 0xe4f4;
                case IconID.GlassWaterDroplet:                 return 0xe4f5;
                case IconID.Glasses:                           return 0xf530;
                case IconID.Globe:                             return 0xf0ac;
                case IconID.GolfBallTee:                       return 0xf450;
                case IconID.Gopuram:                           return 0xf664;
                case IconID.GraduationCap:                     return 0xf19d;
                case IconID.GreaterThan:                       return 0x003e;
                case IconID.GreaterThanEqual:                  return 0xf532;
                case IconID.Grip:                              return 0xf58d;
                case IconID.GripLines:                         return 0xf7a4;
                case IconID.GripLinesVertical:                 return 0xf7a5;
                case IconID.GripVertical:                      return 0xf58e;
                case IconID.GroupArrowsRotate:                 return 0xe4f6;
                case IconID.GuaraniSign:                       return 0xe19a;
                case IconID.Guitar:                            return 0xf7a6;
                case IconID.Gun:                               return 0xe19b;
                case IconID.H:                                 return 0x0048;
                case IconID.Hammer:                            return 0xf6e3;
                case IconID.Hamsa:                             return 0xf665;
                case IconID.Hand:                              return 0xf256;
                case IconID.HandBackFist:                      return 0xf255;
                case IconID.HandDots:                          return 0xf461;
                case IconID.HandFist:                          return 0xf6de;
                case IconID.HandHolding:                       return 0xf4bd;
                case IconID.HandHoldingDollar:                 return 0xf4c0;
                case IconID.HandHoldingDroplet:                return 0xf4c1;
                case IconID.HandHoldingHand:                   return 0xe4f7;
                case IconID.HandHoldingHeart:                  return 0xf4be;
                case IconID.HandHoldingMedical:                return 0xe05c;
                case IconID.HandLizard:                        return 0xf258;
                case IconID.HandMiddleFinger:                  return 0xf806;
                case IconID.HandPeace:                         return 0xf25b;
                case IconID.HandPointDown:                     return 0xf0a7;
                case IconID.HandPointLeft:                     return 0xf0a5;
                case IconID.HandPointRight:                    return 0xf0a4;
                case IconID.HandPointUp:                       return 0xf0a6;
                case IconID.HandPointer:                       return 0xf25a;
                case IconID.HandScissors:                      return 0xf257;
                case IconID.HandSparkles:                      return 0xe05d;
                case IconID.HandSpock:                         return 0xf259;
                case IconID.Handcuffs:                         return 0xe4f8;
                case IconID.Hands:                             return 0xf2a7;
                case IconID.HandsAslInterpreting:              return 0xf2a3;
                case IconID.HandsBound:                        return 0xe4f9;
                case IconID.HandsBubbles:                      return 0xe05e;
                case IconID.HandsClapping:                     return 0xe1a8;
                case IconID.HandsHolding:                      return 0xf4c2;
                case IconID.HandsHoldingChild:                 return 0xe4fa;
                case IconID.HandsHoldingCircle:                return 0xe4fb;
                case IconID.HandsPraying:                      return 0xf684;
                case IconID.Handshake:                         return 0xf2b5;
                case IconID.HandshakeAngle:                    return 0xf4c4;
                case IconID.HandshakeSimple:                   return 0xf4c6;
                case IconID.HandshakeSimpleSlash:              return 0xe05f;
                case IconID.HandshakeSlash:                    return 0xe060;
                case IconID.Hanukiah:                          return 0xf6e6;
                case IconID.HardDrive:                         return 0xf0a0;
                case IconID.Hashtag:                           return 0x0023;
                case IconID.HatCowboy:                         return 0xf8c0;
                case IconID.HatCowboySide:                     return 0xf8c1;
                case IconID.HatWizard:                         return 0xf6e8;
                case IconID.HeadSideCough:                     return 0xe061;
                case IconID.HeadSideCoughSlash:                return 0xe062;
                case IconID.HeadSideMask:                      return 0xe063;
                case IconID.HeadSideVirus:                     return 0xe064;
                case IconID.Heading:                           return 0xf1dc;
                case IconID.Headphones:                        return 0xf025;
                case IconID.HeadphonesSimple:                  return 0xf58f;
                case IconID.Headset:                           return 0xf590;
                case IconID.Heart:                             return 0xf004;
                case IconID.HeartCircleBolt:                   return 0xe4fc;
                case IconID.HeartCircleCheck:                  return 0xe4fd;
                case IconID.HeartCircleExclamation:            return 0xe4fe;
                case IconID.HeartCircleMinus:                  return 0xe4ff;
                case IconID.HeartCirclePlus:                   return 0xe500;
                case IconID.HeartCircleXmark:                  return 0xe501;
                case IconID.HeartCrack:                        return 0xf7a9;
                case IconID.HeartPulse:                        return 0xf21e;
                case IconID.Helicopter:                        return 0xf533;
                case IconID.HelicopterSymbol:                  return 0xe502;
                case IconID.HelmetSafety:                      return 0xf807;
                case IconID.HelmetUn:                          return 0xe503;
                case IconID.Highlighter:                       return 0xf591;
                case IconID.HillAvalanche:                     return 0xe507;
                case IconID.HillRockslide:                     return 0xe508;
                case IconID.Hippo:                             return 0xf6ed;
                case IconID.HockeyPuck:                        return 0xf453;
                case IconID.HollyBerry:                        return 0xf7aa;
                case IconID.Horse:                             return 0xf6f0;
                case IconID.HorseHead:                         return 0xf7ab;
                case IconID.Hospital:                          return 0xf0f8;
                case IconID.HospitalUser:                      return 0xf80d;
                case IconID.HotTubPerson:                      return 0xf593;
                case IconID.Hotdog:                            return 0xf80f;
                case IconID.Hotel:                             return 0xf594;
                case IconID.Hourglass:                         return 0xf254;
                case IconID.HourglassEnd:                      return 0xf253;
                case IconID.HourglassHalf:                     return 0xf252;
                case IconID.HourglassStart:                    return 0xf251;
                case IconID.House:                             return 0xf015;
                case IconID.HouseChimney:                      return 0xe3af;
                case IconID.HouseChimneyCrack:                 return 0xf6f1;
                case IconID.HouseChimneyMedical:               return 0xf7f2;
                case IconID.HouseChimneyUser:                  return 0xe065;
                case IconID.HouseChimneyWindow:                return 0xe00d;
                case IconID.HouseCircleCheck:                  return 0xe509;
                case IconID.HouseCircleExclamation:            return 0xe50a;
                case IconID.HouseCircleXmark:                  return 0xe50b;
                case IconID.HouseCrack:                        return 0xe3b1;
                case IconID.HouseFire:                         return 0xe50c;
                case IconID.HouseFlag:                         return 0xe50d;
                case IconID.HouseFloodWater:                   return 0xe50e;
                case IconID.HouseFloodWaterCircleArrowRight:   return 0xe50f;
                case IconID.HouseLaptop:                       return 0xe066;
                case IconID.HouseLock:                         return 0xe510;
                case IconID.HouseMedical:                      return 0xe3b2;
                case IconID.HouseMedicalCircleCheck:           return 0xe511;
                case IconID.HouseMedicalCircleExclamation:     return 0xe512;
                case IconID.HouseMedicalCircleXmark:           return 0xe513;
                case IconID.HouseMedicalFlag:                  return 0xe514;
                case IconID.HouseSignal:                       return 0xe012;
                case IconID.HouseTsunami:                      return 0xe515;
                case IconID.HouseUser:                         return 0xe1b0;
                case IconID.HryvniaSign:                       return 0xf6f2;
                case IconID.Hurricane:                         return 0xf751;
                case IconID.I:                                 return 0x0049;
                case IconID.ICursor:                           return 0xf246;
                case IconID.IceCream:                          return 0xf810;
                case IconID.Icicles:                           return 0xf7ad;
                case IconID.Icons:                             return 0xf86d;
                case IconID.IdBadge:                           return 0xf2c1;
                case IconID.IdCard:                            return 0xf2c2;
                case IconID.IdCardClip:                        return 0xf47f;
                case IconID.Igloo:                             return 0xf7ae;
                case IconID.Image:                             return 0xf03e;
                case IconID.ImagePortrait:                     return 0xf3e0;
                case IconID.Images:                            return 0xf302;
                case IconID.Inbox:                             return 0xf01c;
                case IconID.Indent:                            return 0xf03c;
                case IconID.IndianRupeeSign:                   return 0xe1bc;
                case IconID.Industry:                          return 0xf275;
                case IconID.Infinity:                          return 0xf534;
                case IconID.Info:                              return 0xf129;
                case IconID.Italic:                            return 0xf033;
                case IconID.J:                                 return 0x004a;
                case IconID.Jar:                               return 0xe516;
                case IconID.JarWheat:                          return 0xe517;
                case IconID.Jedi:                              return 0xf669;
                case IconID.JetFighter:                        return 0xf0fb;
                case IconID.JetFighterUp:                      return 0xe518;
                case IconID.Joint:                             return 0xf595;
                case IconID.JugDetergent:                      return 0xe519;
                case IconID.K:                                 return 0x004b;
                case IconID.Kaaba:                             return 0xf66b;
                case IconID.Key:                               return 0xf084;
                case IconID.Keyboard:                          return 0xf11c;
                case IconID.Khanda:                            return 0xf66d;
                case IconID.KipSign:                           return 0xe1c4;
                case IconID.KitMedical:                        return 0xf479;
                case IconID.KitchenSet:                        return 0xe51a;
                case IconID.KiwiBird:                          return 0xf535;
                case IconID.L:                                 return 0x004c;
                case IconID.LandMineOn:                        return 0xe51b;
                case IconID.Landmark:                          return 0xf66f;
                case IconID.LandmarkDome:                      return 0xf752;
                case IconID.LandmarkFlag:                      return 0xe51c;
                case IconID.Language:                          return 0xf1ab;
                case IconID.Laptop:                            return 0xf109;
                case IconID.LaptopCode:                        return 0xf5fc;
                case IconID.LaptopFile:                        return 0xe51d;
                case IconID.LaptopMedical:                     return 0xf812;
                case IconID.LariSign:                          return 0xe1c8;
                case IconID.LayerGroup:                        return 0xf5fd;
                case IconID.Leaf:                              return 0xf06c;
                case IconID.LeftLong:                          return 0xf30a;
                case IconID.LeftRight:                         return 0xf337;
                case IconID.Lemon:                             return 0xf094;
                case IconID.LessThan:                          return 0x003c;
                case IconID.LessThanEqual:                     return 0xf537;
                case IconID.LifeRing:                          return 0xf1cd;
                case IconID.Lightbulb:                         return 0xf0eb;
                case IconID.LinesLeaning:                      return 0xe51e;
                case IconID.Link:                              return 0xf0c1;
                case IconID.LinkSlash:                         return 0xf127;
                case IconID.LiraSign:                          return 0xf195;
                case IconID.List:                              return 0xf03a;
                case IconID.ListCheck:                         return 0xf0ae;
                case IconID.ListOl:                            return 0xf0cb;
                case IconID.ListUl:                            return 0xf0ca;
                case IconID.LitecoinSign:                      return 0xe1d3;
                case IconID.LocationArrow:                     return 0xf124;
                case IconID.LocationCrosshairs:                return 0xf601;
                case IconID.LocationDot:                       return 0xf3c5;
                case IconID.LocationPin:                       return 0xf041;
                case IconID.LocationPinLock:                   return 0xe51f;
                case IconID.Lock:                              return 0xf023;
                case IconID.LockOpen:                          return 0xf3c1;
                case IconID.Locust:                            return 0xe520;
                case IconID.Lungs:                             return 0xf604;
                case IconID.LungsVirus:                        return 0xe067;
                case IconID.M:                                 return 0x004d;
                case IconID.Magnet:                            return 0xf076;
                case IconID.MagnifyingGlass:                   return 0xf002;
                case IconID.MagnifyingGlassArrowRight:         return 0xe521;
                case IconID.MagnifyingGlassChart:              return 0xe522;
                case IconID.MagnifyingGlassDollar:             return 0xf688;
                case IconID.MagnifyingGlassLocation:           return 0xf689;
                case IconID.MagnifyingGlassMinus:              return 0xf010;
                case IconID.MagnifyingGlassPlus:               return 0xf00e;
                case IconID.ManatSign:                         return 0xe1d5;
                case IconID.Map:                               return 0xf279;
                case IconID.MapLocation:                       return 0xf59f;
                case IconID.MapLocationDot:                    return 0xf5a0;
                case IconID.MapPin:                            return 0xf276;
                case IconID.Marker:                            return 0xf5a1;
                case IconID.Mars:                              return 0xf222;
                case IconID.MarsAndVenus:                      return 0xf224;
                case IconID.MarsAndVenusBurst:                 return 0xe523;
                case IconID.MarsDouble:                        return 0xf227;
                case IconID.MarsStroke:                        return 0xf229;
                case IconID.MarsStrokeRight:                   return 0xf22b;
                case IconID.MarsStrokeUp:                      return 0xf22a;
                case IconID.MartiniGlass:                      return 0xf57b;
                case IconID.MartiniGlassCitrus:                return 0xf561;
                case IconID.MartiniGlassEmpty:                 return 0xf000;
                case IconID.Mask:                              return 0xf6fa;
                case IconID.MaskFace:                          return 0xe1d7;
                case IconID.MaskVentilator:                    return 0xe524;
                case IconID.MasksTheater:                      return 0xf630;
                case IconID.MattressPillow:                    return 0xe525;
                case IconID.Maximize:                          return 0xf31e;
                case IconID.Medal:                             return 0xf5a2;
                case IconID.Memory:                            return 0xf538;
                case IconID.Menorah:                           return 0xf676;
                case IconID.Mercury:                           return 0xf223;
                case IconID.Message:                           return 0xf27a;
                case IconID.Meteor:                            return 0xf753;
                case IconID.Microchip:                         return 0xf2db;
                case IconID.Microphone:                        return 0xf130;
                case IconID.MicrophoneLines:                   return 0xf3c9;
                case IconID.MicrophoneLinesSlash:              return 0xf539;
                case IconID.MicrophoneSlash:                   return 0xf131;
                case IconID.Microscope:                        return 0xf610;
                case IconID.MillSign:                          return 0xe1ed;
                case IconID.Minimize:                          return 0xf78c;
                case IconID.Minus:                             return 0xf068;
                case IconID.Mitten:                            return 0xf7b5;
                case IconID.Mobile:                            return 0xf3ce;
                case IconID.MobileButton:                      return 0xf10b;
                case IconID.MobileRetro:                       return 0xe527;
                case IconID.MobileScreen:                      return 0xf3cf;
                case IconID.MobileScreenButton:                return 0xf3cd;
                case IconID.MoneyBill:                         return 0xf0d6;
                case IconID.MoneyBill1:                        return 0xf3d1;
                case IconID.MoneyBill1Wave:                    return 0xf53b;
                case IconID.MoneyBillTransfer:                 return 0xe528;
                case IconID.MoneyBillTrendUp:                  return 0xe529;
                case IconID.MoneyBillWave:                     return 0xf53a;
                case IconID.MoneyBillWheat:                    return 0xe52a;
                case IconID.MoneyBills:                        return 0xe1f3;
                case IconID.MoneyCheck:                        return 0xf53c;
                case IconID.MoneyCheckDollar:                  return 0xf53d;
                case IconID.Monument:                          return 0xf5a6;
                case IconID.Moon:                              return 0xf186;
                case IconID.MortarPestle:                      return 0xf5a7;
                case IconID.Mosque:                            return 0xf678;
                case IconID.Mosquito:                          return 0xe52b;
                case IconID.MosquitoNet:                       return 0xe52c;
                case IconID.Motorcycle:                        return 0xf21c;
                case IconID.Mound:                             return 0xe52d;
                case IconID.Mountain:                          return 0xf6fc;
                case IconID.MountainCity:                      return 0xe52e;
                case IconID.MountainSun:                       return 0xe52f;
                case IconID.MugHot:                            return 0xf7b6;
                case IconID.MugSaucer:                         return 0xf0f4;
                case IconID.Music:                             return 0xf001;
                case IconID.N:                                 return 0x004e;
                case IconID.NairaSign:                         return 0xe1f6;
                case IconID.NetworkWired:                      return 0xf6ff;
                case IconID.Neuter:                            return 0xf22c;
                case IconID.Newspaper:                         return 0xf1ea;
                case IconID.NotEqual:                          return 0xf53e;
                case IconID.Notdef:                            return 0xe1fe;
                case IconID.NoteSticky:                        return 0xf249;
                case IconID.NotesMedical:                      return 0xf481;
                case IconID.O:                                 return 0x004f;
                case IconID.ObjectGroup:                       return 0xf247;
                case IconID.ObjectUngroup:                     return 0xf248;
                case IconID.OilCan:                            return 0xf613;
                case IconID.OilWell:                           return 0xe532;
                case IconID.Om:                                return 0xf679;
                case IconID.Otter:                             return 0xf700;
                case IconID.Outdent:                           return 0xf03b;
                case IconID.P:                                 return 0x0050;
                case IconID.Pager:                             return 0xf815;
                case IconID.PaintRoller:                       return 0xf5aa;
                case IconID.Paintbrush:                        return 0xf1fc;
                case IconID.Palette:                           return 0xf53f;
                case IconID.Pallet:                            return 0xf482;
                case IconID.Panorama:                          return 0xe209;
                case IconID.PaperPlane:                        return 0xf1d8;
                case IconID.Paperclip:                         return 0xf0c6;
                case IconID.ParachuteBox:                      return 0xf4cd;
                case IconID.Paragraph:                         return 0xf1dd;
                case IconID.Passport:                          return 0xf5ab;
                case IconID.Paste:                             return 0xf0ea;
                case IconID.Pause:                             return 0xf04c;
                case IconID.Paw:                               return 0xf1b0;
                case IconID.Peace:                             return 0xf67c;
                case IconID.Pen:                               return 0xf304;
                case IconID.PenClip:                           return 0xf305;
                case IconID.PenFancy:                          return 0xf5ac;
                case IconID.PenNib:                            return 0xf5ad;
                case IconID.PenRuler:                          return 0xf5ae;
                case IconID.PenToSquare:                       return 0xf044;
                case IconID.Pencil:                            return 0xf303;
                case IconID.PeopleArrows:                      return 0xe068;
                case IconID.PeopleCarryBox:                    return 0xf4ce;
                case IconID.PeopleGroup:                       return 0xe533;
                case IconID.PeopleLine:                        return 0xe534;
                case IconID.PeoplePulling:                     return 0xe535;
                case IconID.PeopleRobbery:                     return 0xe536;
                case IconID.PeopleRoof:                        return 0xe537;
                case IconID.PepperHot:                         return 0xf816;
                case IconID.Percent:                           return 0x0025;
                case IconID.Person:                            return 0xf183;
                case IconID.PersonArrowDownToLine:             return 0xe538;
                case IconID.PersonArrowUpFromLine:             return 0xe539;
                case IconID.PersonBiking:                      return 0xf84a;
                case IconID.PersonBooth:                       return 0xf756;
                case IconID.PersonBreastfeeding:               return 0xe53a;
                case IconID.PersonBurst:                       return 0xe53b;
                case IconID.PersonCane:                        return 0xe53c;
                case IconID.PersonChalkboard:                  return 0xe53d;
                case IconID.PersonCircleCheck:                 return 0xe53e;
                case IconID.PersonCircleExclamation:           return 0xe53f;
                case IconID.PersonCircleMinus:                 return 0xe540;
                case IconID.PersonCirclePlus:                  return 0xe541;
                case IconID.PersonCircleQuestion:              return 0xe542;
                case IconID.PersonCircleXmark:                 return 0xe543;
                case IconID.PersonDigging:                     return 0xf85e;
                case IconID.PersonDotsFromLine:                return 0xf470;
                case IconID.PersonDress:                       return 0xf182;
                case IconID.PersonDressBurst:                  return 0xe544;
                case IconID.PersonDrowning:                    return 0xe545;
                case IconID.PersonFalling:                     return 0xe546;
                case IconID.PersonFallingBurst:                return 0xe547;
                case IconID.PersonHalfDress:                   return 0xe548;
                case IconID.PersonHarassing:                   return 0xe549;
                case IconID.PersonHiking:                      return 0xf6ec;
                case IconID.PersonMilitaryPointing:            return 0xe54a;
                case IconID.PersonMilitaryRifle:               return 0xe54b;
                case IconID.PersonMilitaryToPerson:            return 0xe54c;
                case IconID.PersonPraying:                     return 0xf683;
                case IconID.PersonPregnant:                    return 0xe31e;
                case IconID.PersonRays:                        return 0xe54d;
                case IconID.PersonRifle:                       return 0xe54e;
                case IconID.PersonRunning:                     return 0xf70c;
                case IconID.PersonShelter:                     return 0xe54f;
                case IconID.PersonSkating:                     return 0xf7c5;
                case IconID.PersonSkiing:                      return 0xf7c9;
                case IconID.PersonSkiingNordic:                return 0xf7ca;
                case IconID.PersonSnowboarding:                return 0xf7ce;
                case IconID.PersonSwimming:                    return 0xf5c4;
                case IconID.PersonThroughWindow:               return 0xe5a9;
                case IconID.PersonWalking:                     return 0xf554;
                case IconID.PersonWalkingArrowLoopLeft:        return 0xe551;
                case IconID.PersonWalkingArrowRight:           return 0xe552;
                case IconID.PersonWalkingDashedLineArrowRight: return 0xe553;
                case IconID.PersonWalkingLuggage:              return 0xe554;
                case IconID.PersonWalkingWithCane:             return 0xf29d;
                case IconID.PesetaSign:                        return 0xe221;
                case IconID.PesoSign:                          return 0xe222;
                case IconID.Phone:                             return 0xf095;
                case IconID.PhoneFlip:                         return 0xf879;
                case IconID.PhoneSlash:                        return 0xf3dd;
                case IconID.PhoneVolume:                       return 0xf2a0;
                case IconID.PhotoFilm:                         return 0xf87c;
                case IconID.PiggyBank:                         return 0xf4d3;
                case IconID.Pills:                             return 0xf484;
                case IconID.PizzaSlice:                        return 0xf818;
                case IconID.PlaceOfWorship:                    return 0xf67f;
                case IconID.Plane:                             return 0xf072;
                case IconID.PlaneArrival:                      return 0xf5af;
                case IconID.PlaneCircleCheck:                  return 0xe555;
                case IconID.PlaneCircleExclamation:            return 0xe556;
                case IconID.PlaneCircleXmark:                  return 0xe557;
                case IconID.PlaneDeparture:                    return 0xf5b0;
                case IconID.PlaneLock:                         return 0xe558;
                case IconID.PlaneSlash:                        return 0xe069;
                case IconID.PlaneUp:                           return 0xe22d;
                case IconID.PlantWilt:                         return 0xe5aa;
                case IconID.PlateWheat:                        return 0xe55a;
                case IconID.Play:                              return 0xf04b;
                case IconID.Plug:                              return 0xf1e6;
                case IconID.PlugCircleBolt:                    return 0xe55b;
                case IconID.PlugCircleCheck:                   return 0xe55c;
                case IconID.PlugCircleExclamation:             return 0xe55d;
                case IconID.PlugCircleMinus:                   return 0xe55e;
                case IconID.PlugCirclePlus:                    return 0xe55f;
                case IconID.PlugCircleXmark:                   return 0xe560;
                case IconID.Plus:                              return 0x002b;
                case IconID.PlusMinus:                         return 0xe43c;
                case IconID.Podcast:                           return 0xf2ce;
                case IconID.Poo:                               return 0xf2fe;
                case IconID.PooStorm:                          return 0xf75a;
                case IconID.Poop:                              return 0xf619;
                case IconID.PowerOff:                          return 0xf011;
                case IconID.Prescription:                      return 0xf5b1;
                case IconID.PrescriptionBottle:                return 0xf485;
                case IconID.PrescriptionBottleMedical:         return 0xf486;
                case IconID.Print:                             return 0xf02f;
                case IconID.PumpMedical:                       return 0xe06a;
                case IconID.PumpSoap:                          return 0xe06b;
                case IconID.PuzzlePiece:                       return 0xf12e;
                case IconID.Q:                                 return 0x0051;
                case IconID.Qrcode:                            return 0xf029;
                case IconID.Question:                          return 0x003f;
                case IconID.QuoteLeft:                         return 0xf10d;
                case IconID.QuoteRight:                        return 0xf10e;
                case IconID.R:                                 return 0x0052;
                case IconID.Radiation:                         return 0xf7b9;
                case IconID.Radio:                             return 0xf8d7;
                case IconID.Rainbow:                           return 0xf75b;
                case IconID.RankingStar:                       return 0xe561;
                case IconID.Receipt:                           return 0xf543;
                case IconID.RecordVinyl:                       return 0xf8d9;
                case IconID.RectangleAd:                       return 0xf641;
                case IconID.RectangleList:                     return 0xf022;
                case IconID.RectangleXmark:                    return 0xf410;
                case IconID.Recycle:                           return 0xf1b8;
                case IconID.Registered:                        return 0xf25d;
                case IconID.Repeat:                            return 0xf363;
                case IconID.Reply:                             return 0xf3e5;
                case IconID.ReplyAll:                          return 0xf122;
                case IconID.Restroom:                          return 0xf7bd;
                case IconID.Retweet:                           return 0xf079;
                case IconID.Ribbon:                            return 0xf4d6;
                case IconID.RightFromBracket:                  return 0xf2f5;
                case IconID.RightLeft:                         return 0xf362;
                case IconID.RightLong:                         return 0xf30b;
                case IconID.RightToBracket:                    return 0xf2f6;
                case IconID.Ring:                              return 0xf70b;
                case IconID.Road:                              return 0xf018;
                case IconID.RoadBarrier:                       return 0xe562;
                case IconID.RoadBridge:                        return 0xe563;
                case IconID.RoadCircleCheck:                   return 0xe564;
                case IconID.RoadCircleExclamation:             return 0xe565;
                case IconID.RoadCircleXmark:                   return 0xe566;
                case IconID.RoadLock:                          return 0xe567;
                case IconID.RoadSpikes:                        return 0xe568;
                case IconID.Robot:                             return 0xf544;
                case IconID.Rocket:                            return 0xf135;
                case IconID.Rotate:                            return 0xf2f1;
                case IconID.RotateLeft:                        return 0xf2ea;
                case IconID.RotateRight:                       return 0xf2f9;
                case IconID.Route:                             return 0xf4d7;
                case IconID.Rss:                               return 0xf09e;
                case IconID.RubleSign:                         return 0xf158;
                case IconID.Rug:                               return 0xe569;
                case IconID.Ruler:                             return 0xf545;
                case IconID.RulerCombined:                     return 0xf546;
                case IconID.RulerHorizontal:                   return 0xf547;
                case IconID.RulerVertical:                     return 0xf548;
                case IconID.RupeeSign:                         return 0xf156;
                case IconID.RupiahSign:                        return 0xe23d;
                case IconID.S:                                 return 0x0053;
                case IconID.SackDollar:                        return 0xf81d;
                case IconID.SackXmark:                         return 0xe56a;
                case IconID.Sailboat:                          return 0xe445;
                case IconID.Satellite:                         return 0xf7bf;
                case IconID.SatelliteDish:                     return 0xf7c0;
                case IconID.ScaleBalanced:                     return 0xf24e;
                case IconID.ScaleUnbalanced:                   return 0xf515;
                case IconID.ScaleUnbalancedFlip:               return 0xf516;
                case IconID.School:                            return 0xf549;
                case IconID.SchoolCircleCheck:                 return 0xe56b;
                case IconID.SchoolCircleExclamation:           return 0xe56c;
                case IconID.SchoolCircleXmark:                 return 0xe56d;
                case IconID.SchoolFlag:                        return 0xe56e;
                case IconID.SchoolLock:                        return 0xe56f;
                case IconID.Scissors:                          return 0xf0c4;
                case IconID.Screwdriver:                       return 0xf54a;
                case IconID.ScrewdriverWrench:                 return 0xf7d9;
                case IconID.Scroll:                            return 0xf70e;
                case IconID.ScrollTorah:                       return 0xf6a0;
                case IconID.SdCard:                            return 0xf7c2;
                case IconID.Section:                           return 0xe447;
                case IconID.Seedling:                          return 0xf4d8;
                case IconID.Server:                            return 0xf233;
                case IconID.Shapes:                            return 0xf61f;
                case IconID.Share:                             return 0xf064;
                case IconID.ShareFromSquare:                   return 0xf14d;
                case IconID.ShareNodes:                        return 0xf1e0;
                case IconID.SheetPlastic:                      return 0xe571;
                case IconID.ShekelSign:                        return 0xf20b;
                case IconID.Shield:                            return 0xf132;
                case IconID.ShieldCat:                         return 0xe572;
                case IconID.ShieldDog:                         return 0xe573;
                case IconID.ShieldHalved:                      return 0xf3ed;
                case IconID.ShieldHeart:                       return 0xe574;
                case IconID.ShieldVirus:                       return 0xe06c;
                case IconID.Ship:                              return 0xf21a;
                case IconID.Shirt:                             return 0xf553;
                case IconID.ShoePrints:                        return 0xf54b;
                case IconID.Shop:                              return 0xf54f;
                case IconID.ShopLock:                          return 0xe4a5;
                case IconID.ShopSlash:                         return 0xe070;
                case IconID.Shower:                            return 0xf2cc;
                case IconID.Shrimp:                            return 0xe448;
                case IconID.Shuffle:                           return 0xf074;
                case IconID.ShuttleSpace:                      return 0xf197;
                case IconID.SignHanging:                       return 0xf4d9;
                case IconID.Signal:                            return 0xf012;
                case IconID.Signature:                         return 0xf5b7;
                case IconID.SignsPost:                         return 0xf277;
                case IconID.SimCard:                           return 0xf7c4;
                case IconID.Sink:                              return 0xe06d;
                case IconID.Sitemap:                           return 0xf0e8;
                case IconID.Skull:                             return 0xf54c;
                case IconID.SkullCrossbones:                   return 0xf714;
                case IconID.Slash:                             return 0xf715;
                case IconID.Sleigh:                            return 0xf7cc;
                case IconID.Sliders:                           return 0xf1de;
                case IconID.Smog:                              return 0xf75f;
                case IconID.Smoking:                           return 0xf48d;
                case IconID.Snowflake:                         return 0xf2dc;
                case IconID.Snowman:                           return 0xf7d0;
                case IconID.Snowplow:                          return 0xf7d2;
                case IconID.Soap:                              return 0xe06e;
                case IconID.Socks:                             return 0xf696;
                case IconID.SolarPanel:                        return 0xf5ba;
                case IconID.Sort:                              return 0xf0dc;
                case IconID.SortDown:                          return 0xf0dd;
                case IconID.SortUp:                            return 0xf0de;
                case IconID.Spa:                               return 0xf5bb;
                case IconID.SpaghettiMonsterFlying:            return 0xf67b;
                case IconID.SpellCheck:                        return 0xf891;
                case IconID.Spider:                            return 0xf717;
                case IconID.Spinner:                           return 0xf110;
                case IconID.Splotch:                           return 0xf5bc;
                case IconID.Spoon:                             return 0xf2e5;
                case IconID.SprayCan:                          return 0xf5bd;
                case IconID.SprayCanSparkles:                  return 0xf5d0;
                case IconID.Square:                            return 0xf0c8;
                case IconID.SquareArrowUpRight:                return 0xf14c;
                case IconID.SquareCaretDown:                   return 0xf150;
                case IconID.SquareCaretLeft:                   return 0xf191;
                case IconID.SquareCaretRight:                  return 0xf152;
                case IconID.SquareCaretUp:                     return 0xf151;
                case IconID.SquareCheck:                       return 0xf14a;
                case IconID.SquareEnvelope:                    return 0xf199;
                case IconID.SquareFull:                        return 0xf45c;
                case IconID.SquareH:                           return 0xf0fd;
                case IconID.SquareMinus:                       return 0xf146;
                case IconID.SquareNfi:                         return 0xe576;
                case IconID.SquareParking:                     return 0xf540;
                case IconID.SquarePen:                         return 0xf14b;
                case IconID.SquarePersonConfined:              return 0xe577;
                case IconID.SquarePhone:                       return 0xf098;
                case IconID.SquarePhoneFlip:                   return 0xf87b;
                case IconID.SquarePlus:                        return 0xf0fe;
                case IconID.SquarePollHorizontal:              return 0xf682;
                case IconID.SquarePollVertical:                return 0xf681;
                case IconID.SquareRootVariable:                return 0xf698;
                case IconID.SquareRss:                         return 0xf143;
                case IconID.SquareShareNodes:                  return 0xf1e1;
                case IconID.SquareUpRight:                     return 0xf360;
                case IconID.SquareVirus:                       return 0xe578;
                case IconID.SquareXmark:                       return 0xf2d3;
                case IconID.StaffSnake:                        return 0xe579;
                case IconID.Stairs:                            return 0xe289;
                case IconID.Stamp:                             return 0xf5bf;
                case IconID.Stapler:                           return 0xe5af;
                case IconID.Star:                              return 0xf005;
                case IconID.StarAndCrescent:                   return 0xf699;
                case IconID.StarHalf:                          return 0xf089;
                case IconID.StarHalfStroke:                    return 0xf5c0;
                case IconID.StarOfDavid:                       return 0xf69a;
                case IconID.StarOfLife:                        return 0xf621;
                case IconID.SterlingSign:                      return 0xf154;
                case IconID.Stethoscope:                       return 0xf0f1;
                case IconID.Stop:                              return 0xf04d;
                case IconID.Stopwatch:                         return 0xf2f2;
                case IconID.Stopwatch20:                       return 0xe06f;
                case IconID.Store:                             return 0xf54e;
                case IconID.StoreSlash:                        return 0xe071;
                case IconID.StreetView:                        return 0xf21d;
                case IconID.Strikethrough:                     return 0xf0cc;
                case IconID.Stroopwafel:                       return 0xf551;
                case IconID.Subscript:                         return 0xf12c;
                case IconID.Suitcase:                          return 0xf0f2;
                case IconID.SuitcaseMedical:                   return 0xf0fa;
                case IconID.SuitcaseRolling:                   return 0xf5c1;
                case IconID.Sun:                               return 0xf185;
                case IconID.SunPlantWilt:                      return 0xe57a;
                case IconID.Superscript:                       return 0xf12b;
                case IconID.Swatchbook:                        return 0xf5c3;
                case IconID.Synagogue:                         return 0xf69b;
                case IconID.Syringe:                           return 0xf48e;
                case IconID.T:                                 return 0x0054;
                case IconID.Table:                             return 0xf0ce;
                case IconID.TableCells:                        return 0xf00a;
                case IconID.TableCellsLarge:                   return 0xf009;
                case IconID.TableColumns:                      return 0xf0db;
                case IconID.TableList:                         return 0xf00b;
                case IconID.TableTennisPaddleBall:             return 0xf45d;
                case IconID.Tablet:                            return 0xf3fb;
                case IconID.TabletButton:                      return 0xf10a;
                case IconID.TabletScreenButton:                return 0xf3fa;
                case IconID.Tablets:                           return 0xf490;
                case IconID.TachographDigital:                 return 0xf566;
                case IconID.Tag:                               return 0xf02b;
                case IconID.Tags:                              return 0xf02c;
                case IconID.Tape:                              return 0xf4db;
                case IconID.Tarp:                              return 0xe57b;
                case IconID.TarpDroplet:                       return 0xe57c;
                case IconID.Taxi:                              return 0xf1ba;
                case IconID.Teeth:                             return 0xf62e;
                case IconID.TeethOpen:                         return 0xf62f;
                case IconID.TemperatureArrowDown:              return 0xe03f;
                case IconID.TemperatureArrowUp:                return 0xe040;
                case IconID.TemperatureEmpty:                  return 0xf2cb;
                case IconID.TemperatureFull:                   return 0xf2c7;
                case IconID.TemperatureHalf:                   return 0xf2c9;
                case IconID.TemperatureHigh:                   return 0xf769;
                case IconID.TemperatureLow:                    return 0xf76b;
                case IconID.TemperatureQuarter:                return 0xf2ca;
                case IconID.TemperatureThreeQuarters:          return 0xf2c8;
                case IconID.TengeSign:                         return 0xf7d7;
                case IconID.Tent:                              return 0xe57d;
                case IconID.TentArrowDownToLine:               return 0xe57e;
                case IconID.TentArrowLeftRight:                return 0xe57f;
                case IconID.TentArrowTurnLeft:                 return 0xe580;
                case IconID.TentArrowsDown:                    return 0xe581;
                case IconID.Tents:                             return 0xe582;
                case IconID.Terminal:                          return 0xf120;
                case IconID.TextHeight:                        return 0xf034;
                case IconID.TextSlash:                         return 0xf87d;
                case IconID.TextWidth:                         return 0xf035;
                case IconID.Thermometer:                       return 0xf491;
                case IconID.ThumbsDown:                        return 0xf165;
                case IconID.ThumbsUp:                          return 0xf164;
                case IconID.Thumbtack:                         return 0xf08d;
                case IconID.Ticket:                            return 0xf145;
                case IconID.TicketSimple:                      return 0xf3ff;
                case IconID.Timeline:                          return 0xe29c;
                case IconID.ToggleOff:                         return 0xf204;
                case IconID.ToggleOn:                          return 0xf205;
                case IconID.Toilet:                            return 0xf7d8;
                case IconID.ToiletPaper:                       return 0xf71e;
                case IconID.ToiletPaperSlash:                  return 0xe072;
                case IconID.ToiletPortable:                    return 0xe583;
                case IconID.ToiletsPortable:                   return 0xe584;
                case IconID.Toolbox:                           return 0xf552;
                case IconID.Tooth:                             return 0xf5c9;
                case IconID.ToriiGate:                         return 0xf6a1;
                case IconID.Tornado:                           return 0xf76f;
                case IconID.TowerBroadcast:                    return 0xf519;
                case IconID.TowerCell:                         return 0xe585;
                case IconID.TowerObservation:                  return 0xe586;
                case IconID.Tractor:                           return 0xf722;
                case IconID.Trademark:                         return 0xf25c;
                case IconID.TrafficLight:                      return 0xf637;
                case IconID.Trailer:                           return 0xe041;
                case IconID.Train:                             return 0xf238;
                case IconID.TrainSubway:                       return 0xf239;
                case IconID.TrainTram:                         return 0xe5b4;
                case IconID.Transgender:                       return 0xf225;
                case IconID.Trash:                             return 0xf1f8;
                case IconID.TrashArrowUp:                      return 0xf829;
                case IconID.TrashCan:                          return 0xf2ed;
                case IconID.TrashCanArrowUp:                   return 0xf82a;
                case IconID.Tree:                              return 0xf1bb;
                case IconID.TreeCity:                          return 0xe587;
                case IconID.TriangleExclamation:               return 0xf071;
                case IconID.Trophy:                            return 0xf091;
                case IconID.Trowel:                            return 0xe589;
                case IconID.TrowelBricks:                      return 0xe58a;
                case IconID.Truck:                             return 0xf0d1;
                case IconID.TruckArrowRight:                   return 0xe58b;
                case IconID.TruckDroplet:                      return 0xe58c;
                case IconID.TruckFast:                         return 0xf48b;
                case IconID.TruckField:                        return 0xe58d;
                case IconID.TruckFieldUn:                      return 0xe58e;
                case IconID.TruckFront:                        return 0xe2b7;
                case IconID.TruckMedical:                      return 0xf0f9;
                case IconID.TruckMonster:                      return 0xf63b;
                case IconID.TruckMoving:                       return 0xf4df;
                case IconID.TruckPickup:                       return 0xf63c;
                case IconID.TruckPlane:                        return 0xe58f;
                case IconID.TruckRampBox:                      return 0xf4de;
                case IconID.Tty:                               return 0xf1e4;
                case IconID.TurkishLiraSign:                   return 0xe2bb;
                case IconID.TurnDown:                          return 0xf3be;
                case IconID.TurnUp:                            return 0xf3bf;
                case IconID.Tv:                                return 0xf26c;
                case IconID.U:                                 return 0x0055;
                case IconID.Umbrella:                          return 0xf0e9;
                case IconID.UmbrellaBeach:                     return 0xf5ca;
                case IconID.Underline:                         return 0xf0cd;
                case IconID.UniversalAccess:                   return 0xf29a;
                case IconID.Unlock:                            return 0xf09c;
                case IconID.UnlockKeyhole:                     return 0xf13e;
                case IconID.UpDown:                            return 0xf338;
                case IconID.UpDownLeftRight:                   return 0xf0b2;
                case IconID.UpLong:                            return 0xf30c;
                case IconID.UpRightAndDownLeftFromCenter:      return 0xf424;
                case IconID.UpRightFromSquare:                 return 0xf35d;
                case IconID.Upload:                            return 0xf093;
                case IconID.User:                              return 0xf007;
                case IconID.UserAstronaut:                     return 0xf4fb;
                case IconID.UserCheck:                         return 0xf4fc;
                case IconID.UserClock:                         return 0xf4fd;
                case IconID.UserDoctor:                        return 0xf0f0;
                case IconID.UserGear:                          return 0xf4fe;
                case IconID.UserGraduate:                      return 0xf501;
                case IconID.UserGroup:                         return 0xf500;
                case IconID.UserInjured:                       return 0xf728;
                case IconID.UserLarge:                         return 0xf406;
                case IconID.UserLargeSlash:                    return 0xf4fa;
                case IconID.UserLock:                          return 0xf502;
                case IconID.UserMinus:                         return 0xf503;
                case IconID.UserNinja:                         return 0xf504;
                case IconID.UserNurse:                         return 0xf82f;
                case IconID.UserPen:                           return 0xf4ff;
                case IconID.UserPlus:                          return 0xf234;
                case IconID.UserSecret:                        return 0xf21b;
                case IconID.UserShield:                        return 0xf505;
                case IconID.UserSlash:                         return 0xf506;
                case IconID.UserTag:                           return 0xf507;
                case IconID.UserTie:                           return 0xf508;
                case IconID.UserXmark:                         return 0xf235;
                case IconID.Users:                             return 0xf0c0;
                case IconID.UsersBetweenLines:                 return 0xe591;
                case IconID.UsersGear:                         return 0xf509;
                case IconID.UsersLine:                         return 0xe592;
                case IconID.UsersRays:                         return 0xe593;
                case IconID.UsersRectangle:                    return 0xe594;
                case IconID.UsersSlash:                        return 0xe073;
                case IconID.UsersViewfinder:                   return 0xe595;
                case IconID.Utensils:                          return 0xf2e7;
                case IconID.V:                                 return 0x0056;
                case IconID.VanShuttle:                        return 0xf5b6;
                case IconID.Vault:                             return 0xe2c5;
                case IconID.VectorSquare:                      return 0xf5cb;
                case IconID.Venus:                             return 0xf221;
                case IconID.VenusDouble:                       return 0xf226;
                case IconID.VenusMars:                         return 0xf228;
                case IconID.Vest:                              return 0xe085;
                case IconID.VestPatches:                       return 0xe086;
                case IconID.Vial:                              return 0xf492;
                case IconID.VialCircleCheck:                   return 0xe596;
                case IconID.VialVirus:                         return 0xe597;
                case IconID.Vials:                             return 0xf493;
                case IconID.Video:                             return 0xf03d;
                case IconID.VideoSlash:                        return 0xf4e2;
                case IconID.Vihara:                            return 0xf6a7;
                case IconID.Virus:                             return 0xe074;
                case IconID.VirusCovid:                        return 0xe4a8;
                case IconID.VirusCovidSlash:                   return 0xe4a9;
                case IconID.VirusSlash:                        return 0xe075;
                case IconID.Viruses:                           return 0xe076;
                case IconID.Voicemail:                         return 0xf897;
                case IconID.Volcano:                           return 0xf770;
                case IconID.Volleyball:                        return 0xf45f;
                case IconID.VolumeHigh:                        return 0xf028;
                case IconID.VolumeLow:                         return 0xf027;
                case IconID.VolumeOff:                         return 0xf026;
                case IconID.VolumeXmark:                       return 0xf6a9;
                case IconID.VrCardboard:                       return 0xf729;
                case IconID.W:                                 return 0x0057;
                case IconID.WalkieTalkie:                      return 0xf8ef;
                case IconID.Wallet:                            return 0xf555;
                case IconID.WandMagic:                         return 0xf0d0;
                case IconID.WandMagicSparkles:                 return 0xe2ca;
                case IconID.WandSparkles:                      return 0xf72b;
                case IconID.Warehouse:                         return 0xf494;
                case IconID.Water:                             return 0xf773;
                case IconID.WaterLadder:                       return 0xf5c5;
                case IconID.WaveSquare:                        return 0xf83e;
                case IconID.WeightHanging:                     return 0xf5cd;
                case IconID.WeightScale:                       return 0xf496;
                case IconID.WheatAwn:                          return 0xe2cd;
                case IconID.WheatAwnCircleExclamation:         return 0xe598;
                case IconID.Wheelchair:                        return 0xf193;
                case IconID.WheelchairMove:                    return 0xe2ce;
                case IconID.WhiskeyGlass:                      return 0xf7a0;
                case IconID.Wifi:                              return 0xf1eb;
                case IconID.Wind:                              return 0xf72e;
                case IconID.WindowMaximize:                    return 0xf2d0;
                case IconID.WindowMinimize:                    return 0xf2d1;
                case IconID.WindowRestore:                     return 0xf2d2;
                case IconID.WineBottle:                        return 0xf72f;
                case IconID.WineGlass:                         return 0xf4e3;
                case IconID.WineGlassEmpty:                    return 0xf5ce;
                case IconID.WonSign:                           return 0xf159;
                case IconID.Worm:                              return 0xe599;
                case IconID.Wrench:                            return 0xf0ad;
                case IconID.X:                                 return 0x0058;
                case IconID.XRay:                              return 0xf497;
                case IconID.Xmark:                             return 0xf00d;
                case IconID.XmarksLines:                       return 0xe59a;
                case IconID.Y:                                 return 0x0059;
                case IconID.YenSign:                           return 0xf157;
                case IconID.YinYang:                           return 0xf6ad;
                case IconID.Z:                                 return 0x005a;
            }
            return 0;
        }

        public static string FormatIconID( IconID iid )
        {
            return char.ConvertFromUtf32( IconIDToUtf(iid) );
        }


        private PrivateFontCollection                      _PrivateFonts = new PrivateFontCollection();
        private List<FontInternalData>           _LoadedFontInternalData = new List<FontInternalData>();

        private Dictionary<FontChoice, FontInstance>   _FontDataByWeight = new Dictionary<FontChoice, FontInstance>();
        private FontInstance                               _FontDataIcon = null;


        private FontLibrary()
        {
            var baseResourcePath = GetType().Namespace + ".Resources.";

            // symbol font library from Font Awesome
            LoadFontFromResource( baseResourcePath + "FontAwesome6.otf" );
            _FontDataIcon = new FontInstance( FindFamilyByName( "Font Awesome 6 Free Solid" ), FontStyle.Regular );


            // manually load and register the fonts into this process
            var fontChoiceType = typeof( FontChoice );
            foreach ( FontChoice fst in Enum.GetValues( fontChoiceType ) )
            {
                LoadFontFromResource( $"{baseResourcePath}{Enum.GetName(fontChoiceType, fst)}.ttf" );
            }

            {
                FontFamily muliRegular  = FindFamilyByName( "Inter" );

                _FontDataByWeight.Add( FontChoice.Regular,           new FontInstance( muliRegular, FontStyle.Regular ) );
                _FontDataByWeight.Add( FontChoice.Italic,            new FontInstance( muliRegular, FontStyle.Italic ) );
                _FontDataByWeight.Add( FontChoice.Bold,              new FontInstance( muliRegular, FontStyle.Bold ) );
                _FontDataByWeight.Add( FontChoice.BoldItalic,        new FontInstance( muliRegular, FontStyle.Bold | FontStyle.Italic ) );
            }
            {
                FontFamily muliLight    = FindFamilyByName( "Inter Extra Light BETA" );

                _FontDataByWeight.Add( FontChoice.Light,             new FontInstance( muliLight, FontStyle.Regular ) );
                _FontDataByWeight.Add( FontChoice.LightItalic,       new FontInstance( muliLight, FontStyle.Italic ) );
            }
            {
                FontFamily muliSemiBold = FindFamilyByName( "Inter Semi Bold" );

                _FontDataByWeight.Add( FontChoice.SemiBold,          new FontInstance( muliSemiBold, FontStyle.Regular ) );
                _FontDataByWeight.Add( FontChoice.SemiBoldItalic,    new FontInstance( muliSemiBold, FontStyle.Italic ) );
            }
            {
                FontFamily condensed    = FindFamilyByName( "Roboto Condensed" );

                _FontDataByWeight.Add( FontChoice.CondensedRegular,  new FontInstance( condensed, FontStyle.Regular ) );
                _FontDataByWeight.Add( FontChoice.CondensedItalic,   new FontInstance( condensed, FontStyle.Italic ) );
                _FontDataByWeight.Add( FontChoice.CondensedBold,     new FontInstance( condensed, FontStyle.Bold ) );
            }
            {
                FontFamily ptSans       = FindFamilyByName( "Fira Code" );

                _FontDataByWeight.Add( FontChoice.Monospace,         new FontInstance(ptSans, FontStyle.Regular ) );
            }
            {
                FontFamily ptSans       = FindFamilyByName( "Bebas Neue" );

                _FontDataByWeight.Add( FontChoice.Rigid,             new FontInstance( ptSans, FontStyle.Regular ) );
            }
        }


        private void LoadFontFromResource( string resourceName )
        {
            Stream fontStream = null;

            try
            {
                fontStream = GetType().Assembly.GetManifestResourceStream( resourceName );
                var bytes = (int) fontStream.Length;

                IntPtr data     = Marshal.AllocCoTaskMem(bytes);
                var    fontData = new byte[bytes];
                fontStream.Read( fontData, 0, bytes );
                Marshal.Copy( fontData, 0, data, bytes );


                // push the data into our local font repository
                _PrivateFonts.AddMemoryFont( data, bytes );

                uint   cFonts           = 0;
                IntPtr registeredHandle = WinGdi.AddFontMemResourceEx(data, (uint) fontData.Length, IntPtr.Zero, ref cFonts);

                // track all the data used to load the fonts and register them
                _LoadedFontInternalData.Add( new FontInternalData( data, registeredHandle ) );
            }
            finally
            {
                fontStream?.Dispose();
            }
        }

        private FontFamily FindFamilyByName( string fontName )
        {
            return _PrivateFonts.Families.Single( s => s.Name.Equals( fontName ) );
        }

        public void Dispose()
        {
            foreach ( FontInternalData fdi in _LoadedFontInternalData )
                fdi.Dispose();

            foreach ( FontInstance fi in _FontDataByWeight.Values )
                fi.Dispose();

            _FontDataIcon.Dispose();
            _PrivateFonts.Dispose();
        }
    }

    internal sealed class FontInternalData : IDisposable
    {
        readonly IntPtr RawData   = IntPtr.Zero;
        readonly IntPtr RegHandle = IntPtr.Zero;

        public FontInternalData( IntPtr raw, IntPtr regHandle )
        {
            RawData   = raw;
            RegHandle = regHandle;
        }

        public void Dispose()
        {
            if ( RawData != IntPtr.Zero )
                Marshal.FreeCoTaskMem( RawData );

            if ( RegHandle != IntPtr.Zero )
                WinGdi.RemoveFontMemResourceEx( RegHandle );
        }
    }
}
