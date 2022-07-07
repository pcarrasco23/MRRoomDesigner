using Microsoft.MixedReality.Toolkit.Examples.Demos;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemToSpawn;
    public bool UseGravity;
    private GameObject spawnedItem;

    public void SpawnItem()
    {
        var spawnedParentGameObject = Instantiate(new GameObject(), new Vector3(2.0f, 0.2f, 1.0f), Quaternion.identity);

        spawnedItem = Instantiate(itemToSpawn, spawnedParentGameObject.transform);

        if (UseGravity)
        {
            var rigidBody = spawnedItem.AddComponent<Rigidbody>();
            rigidBody.mass = 1;
            rigidBody.drag = 0;
            rigidBody.angularDrag = 0f;
            rigidBody.useGravity = UseGravity;

            rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        spawnedItem.AddComponent<NearInteractionGrabbable>();

        var objectManipulator = spawnedItem.AddComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>();

        var rotationAxisConstraint = spawnedItem.AddComponent<RotationAxisConstraint>();
        rotationAxisConstraint.ConstraintOnRotation = Microsoft.MixedReality.Toolkit.Utilities.AxisFlags.XAxis | Microsoft.MixedReality.Toolkit.Utilities.AxisFlags.ZAxis;
        spawnedItem.AddComponent<ConstraintManager>();

        var boundsControl = spawnedItem.AddComponent<BoundsControl>();
        boundsControl.BoundsControlActivation = Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes.BoundsControlActivationType.ActivateByProximityAndPointer;
        boundsControl.ScaleHandlesConfig.HandleSize = 0.05f;
        boundsControl.RotationHandlesConfig.HandleSize = 0.05f;

        var linksConfiguration = new LinksConfiguration();
        linksConfiguration.ShowWireFrame = false;
        boundsControl.LinksConfig = linksConfiguration;

        var minMaxConstraint = spawnedItem.AddComponent<MinMaxScaleConstraint>();
        minMaxConstraint.RelativeToInitialState = true;
        minMaxConstraint.ScaleMinimum = 0.2f;
        minMaxConstraint.ScaleMaximum = 5.0f;

        spawnedItem.AddComponent<CursorContextObjectManipulator>();
    }
}
