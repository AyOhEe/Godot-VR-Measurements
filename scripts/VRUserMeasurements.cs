using Godot;
using System;

public static class VRUserMeasurements
{
    public enum UserBuildType
    {
        Masculine,
        Feminine,
        Androgynous,
    }

    /*-----STATIC VARIABLES-----*/
    public static MeasurementsAutoload _Instance { get; private set; }

    //the following code is ugly, but makes accessing measurements from C# pretty.
    //i'm frankly okay with this as it stands, but a cleaner implementation would be nice...
    public static float PlayerHeight
    {
        get { return _Instance.PlayerHeight; }
        set { _Instance.PlayerHeight = value; }
    }
    //corrective factor for spatial tracking
    public static float TrackedOffset
    {
        get { return _Instance.TrackedOffset; }
        set { _Instance.TrackedOffset = value; }
    }
    public static float Armspan
    {
        get { return _Instance.Armspan; }
        set { _Instance.Armspan = value; }
    }
    //body type of user
    public static UserBuildType BuildType
    {
        get { return _Instance.BuildType; }
        set { _Instance.BuildType = value; }
    }


    public static float Spine
    {
        get { return _Instance.Spine; }
        set { _Instance.Spine = value; }
    }
    public static float Thigh
    {
        get { return _Instance.Thigh; }
        set { _Instance.Thigh = value; }
    }
    public static float Shin
    {
        get { return _Instance.Shin; }
        set { _Instance.Shin = value; }
    }
    public static float Inseam
    {
        get { return _Instance.Inseam; }
        set { _Instance.Inseam = value; }
    }


    public static float Clavicle
    {
        get { return _Instance.Clavicle; }
        set { _Instance.Clavicle = value; }
    }
    public static float Arm
    {
        get { return _Instance.Arm; }
        set { _Instance.Arm = value; }
    }
    public static float Forearm
    {
        get { return _Instance.Forearm; }
        set { _Instance.Forearm = value; }
    }


    public static float Waist
    {
        get { return _Instance.Waist; }
        set { _Instance.Waist = value; }
    }
    public static float Hips
    {
        get { return _Instance.Hips; }
        set { _Instance.Hips = value; }
    }
    public static float Underbust
    {
        get { return _Instance.Underbust; }
        set { _Instance.Underbust = value; }
    }
    public static float Chest
    {
        get { return _Instance.Chest; }
        set { _Instance.Chest = value; }
    }


    public static Vector3 TrackedOffsetVector => _Instance.TrackedOffsetVector;


    public static void Register(MeasurementsAutoload instance)
    {
        if (_Instance != null)
        {
            instance.QueueFree();
            throw new InvalidOperationException("Only one MeasurementsAutoload may exist!");
        }

        _Instance = instance;
    }
}
