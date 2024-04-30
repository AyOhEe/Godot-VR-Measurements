using Godot;
using System;

public partial class CorrectedCameraRig : CameraRig
{
    public Vector3 RealOrigin { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        RealOrigin = GlobalPosition;
        ResetPosition();
    }

    public void SetRealOrigin(Vector3 v)
    {
        RealOrigin = v;
        ResetPosition();
    }


    public override void _EnterTree()
    {
        VRUserMeasurements._Instance.Connect(
            MeasurementsAutoload.SignalName.MeasurementsChange,
            new Callable(this, "ResetPosition")
        );
    }
    public override void _ExitTree()
    {
        VRUserMeasurements._Instance.Disconnect(
            MeasurementsAutoload.SignalName.MeasurementsChange,
            new Callable(this, "ResetPosition")
        );
    }

    public void ResetPosition()
    {
        GlobalPosition = RealOrigin + VRUserMeasurements.TrackedOffsetVector;
    }
}
