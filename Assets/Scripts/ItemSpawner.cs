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
    public GameObject removeButtonPrefab;
    public bool UseGravity;
    private GameObject removeButton;
    private bool hideRemoveButton;
    private float timeRemainHideRemove = 5;
    private GameObject spawnedItem;

    public void Update()
    {
        if (removeButton != null)
        {
            if (hideRemoveButton)
            {
                if (timeRemainHideRemove > 0)
                {
                    timeRemainHideRemove -= Time.deltaTime;
                }
                else
                {
                    removeButton.SetActive(false);
                }
            }
        }
    }

    public void SpawnItem()
    {
        var spawnedParentGameObject = Instantiate(new GameObject(), new Vector3(0, 0.2f, 1.0f), Quaternion.identity);

        spawnedItem = Instantiate(itemToSpawn, spawnedParentGameObject.transform);

        var rigidBody = spawnedItem.AddComponent<Rigidbody>();
        rigidBody.mass = 1;
        rigidBody.drag = 0;
        rigidBody.angularDrag = 0.05f;
        rigidBody.useGravity = UseGravity;

        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        spawnedItem.AddComponent<NearInteractionGrabbable>();

        var objectManipulator = spawnedItem.AddComponent<Microsoft.MixedReality.Toolkit.UI.ObjectManipulator>();
        objectManipulator.OnHoverEntered.AddListener(ManipulatorHoverEntered);
        objectManipulator.OnHoverExited.AddListener(ManipulatorHoverExited);

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

        removeButton = Instantiate(removeButtonPrefab, spawnedItem.transform);
        var boxCollider = spawnedItem.transform.GetComponentInChildren<BoxCollider>();

        // Rescale remove button
        float size = removeButton.GetComponentInChildren<Renderer>().bounds.size.y;
        Vector3 rescale = removeButton.transform.localScale;
        rescale.y = 0.1f * rescale.y / size;
        rescale.x = 0.1f * rescale.x / size;
        rescale.z = 0.1f * rescale.z / size;
        removeButton.transform.localScale = rescale;

        removeButton.transform.position = new Vector3(spawnedItem.transform.position.x, 1.25f, spawnedItem.transform.position.z);
        removeButton.SetActive(false);
    }

    private void ManipulatorHoverEntered(ManipulationEventData manipEvent)
    {
        hideRemoveButton = false;
        removeButton.SetActive(true);
    }

    private void ManipulatorHoverExited(ManipulationEventData manipEvent)
    {
        hideRemoveButton = true;
        timeRemainHideRemove = 5;
    }
}
