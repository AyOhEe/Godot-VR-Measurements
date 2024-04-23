using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class MeasurementsAutoload : Node
{
    public enum UserBuildType
    {
        Masculine,
        Feminine,
        Androgynous,
    }


    const string MEASUREMENTS_CONFIG_PATH = "user://user_measurements.cfg";


    [Signal] public delegate void MeasurementsChangeEventHandler();


    [ExportCategory("Naive estimations")]
    [Export] public float PlayerHeight { get; private set; }
    //corrective factor for spatial tracking
    [Export] public float TrackedOffset { get; private set; }
    [Export] public float Armspan { get; private set; }
    //body type of user
    [Export] public UserBuildType BuildType {get; private set;}


    [ExportCategory("Vertical sections")]
    [Export] public float Spine { get; private set; }
    [Export] public float Thigh { get; private set; }
    [Export] public float Shin { get; private set; }
    [Export] public float Inseam { get; private set; }


    [ExportCategory("Upper body")]
    [Export] public float Clavicle { get; private set; }
    [Export] public float Arm { get; private set; }
    [Export] public float Forearm { get; private set; }


    [ExportCategory("Core measurements")]
    [Export] public float Waist { get; private set; }
    [Export] public float Hips { get; private set; }
    [Export] public float Underbust { get; private set; }
    [Export] public float Chest { get; private set; }


    //corrective factor for spatial tracking
    //TrackedPosition + TrackedOffsetVector = location of tracker relative to their actual floor
    public Vector3 TrackedOffsetVector { get { return Vector3.Up * TrackedOffset; } }


    public override void _EnterTree()
    {
        CreateConfigIfMissing();

        ConfigFile configFile;
        if(!OpenConfigFile(out configFile))
        {
            GD.PushError("Cannot load user measurements.");
            return;
        }

        //naive estimations
        PlayerHeight = (float)configFile.GetValue("Measurements", "PlayerHeight", Estimations.PlayerHeight(this));
        TrackedOffset = (float)configFile.GetValue("Measurements", "TrackedOffset", Estimations.TrackedOffset(this));
        Armspan = (float)configFile.GetValue("Measurements", "Armspan", Estimations.Armspan(this));
        BuildType = (UserBuildType)(int)configFile.GetValue("Measurements", "BuildType", (int)UserBuildType.Androgynous);

        //vertical sections
        Spine = (float)configFile.GetValue("Measurements", "Spine", Estimations.Spine(this));
        Thigh = (float)configFile.GetValue("Measurements", "Thigh", Estimations.Thigh(this));
        Shin = (float)configFile.GetValue("Measurements", "Shin", Estimations.Shin(this));
        Inseam = (float)configFile.GetValue("Measurements", "Inseam", Estimations.Inseam(this));

        //upper body
        Clavicle = (float)configFile.GetValue("Measurements", "Clavicle", Estimations.Clavicle(this));
        Arm = (float)configFile.GetValue("Measurements", "Arm", Estimations.Arm(this));
        Forearm = (float)configFile.GetValue("Measurements", "Forearm", Estimations.Forearm(this));

        //core measurements
        Hips = (float)configFile.GetValue("Measurements", "Hips", Estimations.Hips(this));
        Waist = (float)configFile.GetValue("Measurements", "Waist", Estimations.Waist(this));
        Underbust = (float)configFile.GetValue("Measurements", "Underbust", Estimations.Underbust(this));
        Chest = (float)configFile.GetValue("Measurements", "Chest", Estimations.Chest(this));
    }

    public void CreateConfigIfMissing()
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
    public bool OpenConfigFile(out ConfigFile config)
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
    public void SaveConfig()
    {
        ConfigFile configFile;
        if (!OpenConfigFile(out configFile))
        {
            GD.PushError("Cannot save user measurements.");
            return;
        }

        //naive estimations
        configFile.SetValue("Measurements", "PlayerHeight", PlayerHeight);
        configFile.SetValue("Measurements", "TrackedOffset", TrackedOffset);
        configFile.SetValue("Measurements", "Armspan", Armspan);
        configFile.SetValue("Measurements", "BuildType", (int)BuildType);

        //vertical sections
        configFile.SetValue("Measurements", "Spine", Spine);
        configFile.SetValue("Measurements", "Thigh", Thigh);
        configFile.SetValue("Measurements", "Shin", Shin);
        configFile.SetValue("Measurements", "Inseam", Inseam);

        //upper body
        configFile.SetValue("Measurements", "Clavicle", Clavicle);
        configFile.SetValue("Measurements", "Arm", Arm);
        configFile.SetValue("Measurements", "Forearm", Forearm);

        //core measurements
        configFile.SetValue("Measurements", "Hips", Hips);
        configFile.SetValue("Measurements", "Waist", Waist);
        configFile.SetValue("Measurements", "Chest", Chest);
        configFile.SetValue("Measurements", "Underbust", Underbust);

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
            return measurements.PlayerHeight;
        }

        public static UserBuildType BuildType(MeasurementsAutoload measurements)
        {
            return UserBuildType.Androgynous;
        }
        #endregion
        #region Vertical sections
        public static float Spine(MeasurementsAutoload measurements)
        {
            return measurements.PlayerHeight * 0.33f;
        }

        public static float Thigh(MeasurementsAutoload measurements)
        {
            return measurements.PlayerHeight * 0.3f;
        }

        public static float Shin(MeasurementsAutoload measurements)
        {
            return measurements.Thigh * 0.76f;
        }

        public static float Inseam(MeasurementsAutoload measurements)
        {
            return measurements.Thigh + measurements.Shin;
        }
        #endregion
        #region Upper body
        public static float Clavicle(MeasurementsAutoload measurements)
        {
            return measurements.Armspan * 0.1f;
        }

        public static float Arm(MeasurementsAutoload measurements)
        {
            return measurements.Clavicle * 1.45f;
        }

        public static float Forearm(MeasurementsAutoload measurements)
        {
            return measurements.Clavicle * 1.4f;
        }
        #endregion
        #region core measurements
        public static float Hips(MeasurementsAutoload measurements)
        {
            //my source is that i made it the fuck up
            switch (measurements.BuildType)
            {
                case UserBuildType.Masculine:
                    return measurements.PlayerHeight * 0.5f;
                case UserBuildType.Feminine:
                    return measurements.PlayerHeight * 0.55f;
                case UserBuildType.Androgynous:
                    return measurements.PlayerHeight * 0.525f;
            }
            return measurements.PlayerHeight * 0.525f;
        }

        public static float Waist(MeasurementsAutoload measurements)
        {
            switch (measurements.BuildType)
            {
                case UserBuildType.Masculine:
                    return measurements.Hips;
                case UserBuildType.Feminine:
                    return measurements.Hips * 0.8f;
                case UserBuildType.Androgynous:
                    return measurements.Hips * 0.95f;
            }
            return measurements.Hips * 0.95f;
        }

        public static float Underbust(MeasurementsAutoload measurements)
        {
            return (measurements.Hips + measurements.Waist) / 2;
        }

        public static float Chest(MeasurementsAutoload measurements)
        {
            GD.Print("Chest estimator called");
            switch (measurements.BuildType)
            {
                case UserBuildType.Masculine:
                case UserBuildType.Androgynous:
                    return measurements.Underbust;
                case UserBuildType.Feminine:
                    return measurements.Underbust + 0.11f;
            }
            return measurements.Underbust;
        }

        #endregion
    }
}
