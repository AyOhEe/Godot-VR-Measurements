using Godot;
using System;

public partial class MeasurementsAutoload : Node
{
    const string MEASUREMENTS_CONFIG_PATH = "user://user_measurements.cfg";
    public enum UserBuildType
    {
        Masculine,
        Feminine,
        Androgynous,
    }

    /*-----STATIC VARIABLES-----*/
    private static MeasurementsAutoload _Instance;

    public static float PlayerHeight { get { return _Instance._PlayerHeight; } set { _Instance._PlayerHeight = value; } }
    //corrective factor for spatial tracking
    public static float TrackedOffset { get { return _Instance._TrackedOffset; } set { _Instance._TrackedOffset = value; } }
    public static float Armspan { get { return _Instance._Armspan; } set { _Instance._Armspan = value; } }
    //body type of user
    public static UserBuildType BuildType { get { return _Instance._BuildType; } set { _Instance._BuildType = value; } }


    public static float Spine { get { return _Instance._Spine; } set { _Instance._Spine = value; } }
    public static float Thigh { get { return _Instance._Thigh; } set { _Instance._Thigh = value; } }
    public static float Shin { get { return _Instance._Shin; } set { _Instance._Shin = value; } }
    public static float Inseam { get { return _Instance._Inseam; } set { _Instance._Inseam = value; } }


    public static float Clavicle { get { return _Instance._Clavicle; } set { _Instance._Clavicle = value; } }
    public static float Arm { get { return _Instance._Arm; } set { _Instance._Arm = value; } }
    public static float Forearm { get { return _Instance._Forearm; } set { _Instance._Forearm = value; } }


    public static float Waist { get { return _Instance._Waist; } set { _Instance._Waist = value; } }
    public static float Hips { get { return _Instance._Hips; } set { _Instance._Hips = value; } }
    public static float Underbust { get { return _Instance._Underbust; } set { _Instance._Underbust = value; } }
    public static float Chest { get { return _Instance._Chest; } set { _Instance._Chest = value; } }


    public static Vector3 TrackedOffsetVector => _Instance._TrackedOffsetVector;


    /*-----INSTANCE VARIABLES-----*/
    [Signal] public delegate void MeasurementsChangeEventHandler();


    [ExportCategory("Naive estimations")]
    [Export] private float _PlayerHeight;
    //corrective factor for spatial tracking
    [Export] private float _TrackedOffset;
    [Export] private float _Armspan;
    //body type of user
    [Export] private UserBuildType _BuildType;


    [ExportCategory("Vertical sections")]
    [Export] private float _Spine;
    [Export] private float _Thigh;
    [Export] private float _Shin;
    [Export] private float _Inseam;


    [ExportCategory("Upper body")]
    [Export] private float _Clavicle;
    [Export] private float _Arm;
    [Export] private float _Forearm;


    [ExportCategory("Core measurements")]
    [Export] private float _Waist;
    [Export] private float _Hips;
    [Export] private float _Underbust;
    [Export] private float _Chest;


    //corrective factor for spatial tracking
    //TrackedPosition + TrackedOffsetVector = location of tracker relative to their actual floor
    private Vector3 _TrackedOffsetVector { get { return Vector3.Up * _TrackedOffset; } }


    public override void _EnterTree()
    {
        //prevent two instances of this script existing
        if(_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            //Dangerous! If two MeasurementsAutoloads are registered as autoloads,
            //  this *WILL* crash the game before it can even start!
            QueueFree();
        }

        LoadMeasurements();
    }

    public void LoadMeasurements()
    {
        CreateConfigIfMissing();

        ConfigFile configFile;
        if (!OpenConfigFile(out configFile))
        {
            GD.PushError("Cannot load user measurements.");
            return;
        }

        //naive estimations
        _PlayerHeight = (float)configFile.GetValue("Measurements", "PlayerHeight", Estimations.PlayerHeight(this));
        _TrackedOffset = (float)configFile.GetValue("Measurements", "TrackedOffset", Estimations.TrackedOffset(this));
        _Armspan = (float)configFile.GetValue("Measurements", "Armspan", Estimations.Armspan(this));
        _BuildType = (UserBuildType)(int)configFile.GetValue("Measurements", "BuildType", (int)UserBuildType.Androgynous);

        //vertical sections
        _Spine = (float)configFile.GetValue("Measurements", "Spine", Estimations.Spine(this));
        _Thigh = (float)configFile.GetValue("Measurements", "Thigh", Estimations.Thigh(this));
        _Shin = (float)configFile.GetValue("Measurements", "Shin", Estimations.Shin(this));
        _Inseam = (float)configFile.GetValue("Measurements", "Inseam", Estimations.Inseam(this));

        //upper body
        _Clavicle = (float)configFile.GetValue("Measurements", "Clavicle", Estimations.Clavicle(this));
        _Arm = (float)configFile.GetValue("Measurements", "Arm", Estimations.Arm(this));
        _Forearm = (float)configFile.GetValue("Measurements", "Forearm", Estimations.Forearm(this));

        //core measurements
        _Hips = (float)configFile.GetValue("Measurements", "Hips", Estimations.Hips(this));
        _Waist = (float)configFile.GetValue("Measurements", "Waist", Estimations.Waist(this));
        _Underbust = (float)configFile.GetValue("Measurements", "Underbust", Estimations.Underbust(this));
        _Chest = (float)configFile.GetValue("Measurements", "Chest", Estimations.Chest(this));
    }

    private void CreateConfigIfMissing()
    {
        ConfigFile config = new ConfigFile();
        Error err = config.Load(MEASUREMENTS_CONFIG_PATH);
        if (err != Error.Ok)
        {
            config = new ConfigFile();
            err = config.Save(MEASUREMENTS_CONFIG_PATH);
        }
    }

    /// <summary>
    /// Opens the ConfigFile containing user measurements
    /// </summary>
    /// <param name="config">New ConfigFile object is stored here</param>
    /// <returns>Returns true if ConfigFile successfully opened</returns>
    private bool OpenConfigFile(out ConfigFile config)
    {
        config = new ConfigFile();
        Error err = config.Load(MEASUREMENTS_CONFIG_PATH);
        if (err != Error.Ok)
        {
            GD.PushError("User measurements file failed to load! ", err);
            config = null;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Saves the user measurements to the ConfigFile containing user measurements
    /// </summary>
    private void SaveConfig()
    {
        ConfigFile configFile;
        if (!OpenConfigFile(out configFile))
        {
            GD.PushError("Cannot save user measurements.");
            return;
        }

        //naive estimations
        configFile.SetValue("Measurements", "PlayerHeight", _PlayerHeight);
        configFile.SetValue("Measurements", "TrackedOffset", _TrackedOffset);
        configFile.SetValue("Measurements", "Armspan", _Armspan);
        configFile.SetValue("Measurements", "BuildType", (int)_BuildType);

        //vertical sections
        configFile.SetValue("Measurements", "Spine", _Spine);
        configFile.SetValue("Measurements", "Thigh", _Thigh);
        configFile.SetValue("Measurements", "Shin", _Shin);
        configFile.SetValue("Measurements", "Inseam", _Inseam);

        //upper body
        configFile.SetValue("Measurements", "Clavicle", _Clavicle);
        configFile.SetValue("Measurements", "Arm", _Arm);
        configFile.SetValue("Measurements", "Forearm", _Forearm);

        //core measurements
        configFile.SetValue("Measurements", "Hips", _Hips);
        configFile.SetValue("Measurements", "Waist", _Waist);
        configFile.SetValue("Measurements", "Chest", _Chest);
        configFile.SetValue("Measurements", "Underbust", _Underbust);

        configFile.Save(MEASUREMENTS_CONFIG_PATH);
    }

    public override void _ExitTree()
    {
        SaveConfig();
    }


    /// <summary>
    /// Estimations for each user measurement.<br></br>
    /// <br></br>
    /// Each estimation presumes that the measurements above are either known
    /// or have already been estimated.
    /// </summary>
    public static class Estimations
    {
        #region Naive estimations
        public static float PlayerHeight(MeasurementsAutoload measurements)
        {
            return 1.70f;
        }

        public static float TrackedOffset(MeasurementsAutoload measurements)
        {
            return 0.00f;
        }

        public static float Armspan(MeasurementsAutoload measurements)
        {
            return measurements._PlayerHeight;
        }

        public static UserBuildType BuildType(MeasurementsAutoload measurements)
        {
            return UserBuildType.Androgynous;
        }
        #endregion
        #region Vertical sections
        public static float Spine(MeasurementsAutoload measurements)
        {
            return measurements._PlayerHeight * 0.33f;
        }

        public static float Thigh(MeasurementsAutoload measurements)
        {
            return measurements._PlayerHeight * 0.3f;
        }

        public static float Shin(MeasurementsAutoload measurements)
        {
            return measurements._Thigh * 0.76f;
        }

        public static float Inseam(MeasurementsAutoload measurements)
        {
            return measurements._Thigh + measurements._Shin;
        }
        #endregion
        #region Upper body
        public static float Clavicle(MeasurementsAutoload measurements)
        {
            return measurements._Armspan * 0.1f;
        }

        public static float Arm(MeasurementsAutoload measurements)
        {
            return measurements._Clavicle * 1.45f;
        }

        public static float Forearm(MeasurementsAutoload measurements)
        {
            return measurements._Clavicle * 1.4f;
        }
        #endregion
        #region core measurements
        public static float Hips(MeasurementsAutoload measurements)
        {
            //my source is that i made it the fuck up
            switch (measurements._BuildType)
            {
                case UserBuildType.Masculine:
                    return measurements._PlayerHeight * 0.5f;
                case UserBuildType.Feminine:
                    return measurements._PlayerHeight * 0.55f;
                case UserBuildType.Androgynous:
                    return measurements._PlayerHeight * 0.525f;
            }
            return measurements._PlayerHeight * 0.525f;
        }

        public static float Waist(MeasurementsAutoload measurements)
        {
            switch (measurements._BuildType)
            {
                case UserBuildType.Masculine:
                    return measurements._Hips;
                case UserBuildType.Feminine:
                    return measurements._Hips * 0.8f;
                case UserBuildType.Androgynous:
                    return measurements._Hips * 0.95f;
            }
            return measurements._Hips * 0.95f;
        }

        public static float Underbust(MeasurementsAutoload measurements)
        {
            return (measurements._Hips + measurements._Waist) / 2;
        }

        public static float Chest(MeasurementsAutoload measurements)
        {
            switch (measurements._BuildType)
            {
                case UserBuildType.Masculine:
                case UserBuildType.Androgynous:
                    return measurements._Underbust;
                case UserBuildType.Feminine:
                    return measurements._Underbust + 0.11f;
            }
            return measurements._Underbust;
        }

        #endregion
    }
}
