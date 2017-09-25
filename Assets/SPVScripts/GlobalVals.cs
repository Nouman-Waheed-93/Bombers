
public static class GlobalVals  {

    public static int AvatarInd = 0;
    public static int CurrPlaneInd = 0;
    public static string PlayerName = "";
    public static string WaterTag = "Water";
    public static string TargetTag = "GroundTarget";
    public static string LockableGTTag = "LockableGrndTrgt";
    public static string BombardmentPTag = "BombardmentPoint";
    public static string PlaneTag = "Plane";
    public static string HUDTag = "HUDCanvas";
    public static string Credits = "Credit";
    public static string PlayerRank = "Rank";
    public static string UnlockedMissions = "UnlockedMissions";
    public static string AircraftPrice = "AircraftPrice";
    public static string PlaneUnlocked = "PlaneUnlocked";
    public static string Medal = "Medal";
    public static string[] EnglishMedalNames = { "Thunder Of Wrath", "Wing Shield", "Ferocity Wheel", "Flying Arrow", "Star Of Valour" };
    public static string[] ArabicMedalNames = { "الرعد من غضب", "درع الجناح", "عجلة الغضب", "تحلق السهم", "نجم الشجاعة" };
    public static string[] FrenchMedalNames = { "Tonnerre de la colère", "Bouclier d'aile", "Roue de férocité", "Flèche volante", "Étoile de courage" };

    public static string[] EnglishMedalDetails = { "nnnn displayed superb airmanship and determination to successfully complete his assigned mission.",
        "nnnn's outstanding skills and courageous initiatives were instrumental in his unit accomplishing its mission objective and inflicting heavy damage upon the enemy.",
        "nnnn, with complete disregard for his personal safety engaged the enemy and successfully accomplished his mission in the face of intense enemy fire.",
        "nnnn fearlessly engaged the enemy in the face of extremely heavy hostile fire.",
        "nnnn aggressively engaged the enemy with complete disregard for his personal safety, and enabled his unit to successfully complete the mission against an enemy force of overwhelming numerical superiority." };
    public static string[] ArabicMedalDetails = { "nnnn عرض إيرمانشيب رائع والعزم على إكمال مهمته المكلفة بنجاح.",
        "كانت مهارات nnnn المتميزة ومبادراته الشجاعة مفيدة في وحدته لتحقيق هدف مهمته وإلحاق أضرار جسيمة بالعدو.",
        "nnnn، مع تجاهل تام لسلامته الشخصية تشارك العدو ونجحت في مهمته في مواجهة النيران العدو الشديد.",
        "nnnn شاركت بلا خوف العدو في مواجهة النار العدائية الثقيلة للغاية.",
        "nnnn شارك العدو بقوة مع تجاهل تام لسلامته الشخصية، وتمكن وحدته بنجاح إكمال المهمة ضد قوة العدو من التفوق العددي الساحق." };
    public static string[] FrenchMedalDetails = { "nnnn a affiché une superbe maîtrise d'esprit et une détermination à accomplir avec succès sa mission assignée.",
        "Les compétences remarquables d'nnnn et les initiatives courageuses ont joué un rôle déterminant dans l'accomplissement de son objectif de mission par l'unité et inflige beaucoup d'ennemies à l'ennemi.",
        "nnnn, avec un mépris total pour sa sécurité personnelle, engagea l'ennemi et réussit sa mission face à un feu ennemi intense.",
        "nnnn attaqua sans crainte l'ennemi face à un feu hostile extrêmement lourd.",
        "nnnn a agressivement engagé l'ennemi avec un mépris total pour sa sécurité personnelle et a permis à son unité de compléter avec succès la mission contre une force ennemie d'une supériorité numérique écrasante." };
    

    public enum SupportedLanguages { Arabic, English, French};
    public static SupportedLanguages SelectedLanguage = SupportedLanguages.English;

    public static string[] OnlineMedalImageLinks = {
        "http://i.imgur.com/Aq1BzvC.png",
        "http://i.imgur.com/8whWaaV.png",
        "http://i.imgur.com/hkOc85q.png",
        "http://i.imgur.com/cqpwcEH.png",
        "http://i.imgur.com/Xsga1D5.png"
    };

    public static string[] OnlineAvatarImageLinks = {
        "http://i.imgur.com/0I0TI0c.png",
        "http://i.imgur.com/CBAH1St.png",
        "http://i.imgur.com/qFOovVf.png",
        "http://i.imgur.com/ZBIVWdz.png",
        "http://i.imgur.com/nanEUjz.png"
    };

    public static string[] EnglishRanks = { "Trainee", "Pilot Officer", "Flying Officer", "Flight Leutenant", "Squadron Leader",
    "Wing Commander", "Group Captain", "Air Commodore"};

    public static string[] FrenchRanks = { "aspirant", "sous-lieutenant", "lieutenant"  , "capitaine"       , "commandant",
    "lieutenant-colonel", "colonel", "général"};

    public static string[] ArabicRanks = { "نقيب" , "رائد" , "مقدم" , "عقيد" , "عميد" , "لواء" , "فريق" , "فريق أول‎‎" };

}
