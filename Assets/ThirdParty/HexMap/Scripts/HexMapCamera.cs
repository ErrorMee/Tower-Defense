﻿using UnityEngine;

public class HexMapCamera : MonoBehaviour {

	public float stickMinZoom, stickMaxZoom;

	public float swivelMinZoom, swivelMaxZoom;

	public float moveSpeedMinZoom, moveSpeedMaxZoom;

	public float rotationSpeed;

    public float offsetMoveCount = 4f;

    Transform swivel, stick;

	public HexGrid grid;

	float zoom = 0f;

	float rotationAngle;

	static HexMapCamera instance;

	public static bool Locked {
		set {
			instance.enabled = !value;
		}
	}

	public static void ValidatePosition () {
		instance.AdjustPosition(0f, 0f);
	}

	void Awake () {
		swivel = transform.GetChild(0);
		stick = swivel.GetChild(0);
	}

	void OnEnable () {
		instance = this;
		ValidatePosition();
	}

	void Update () {
		//float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
		//if (zoomDelta != 0f) {
		//	AdjustZoom(zoomDelta);
		//}

		//float rotationDelta = Input.GetAxis("Rotation");
		//if (rotationDelta != 0f) {
		//	AdjustRotation(rotationDelta);
		//}

		//float xDelta = Input.GetAxis("Horizontal");
		//float zDelta = Input.GetAxis("Vertical");
		//if (xDelta != 0f || zDelta != 0f)
		//{
		//	AdjustPosition(xDelta, zDelta);
		//}
	}

	public void AdjustZoom (float delta) {
		zoom = Mathf.Clamp01(zoom + delta);

		float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
		stick.localPosition = new Vector3(0f, 0f, distance);

		float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
		swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
	}

	public void AdjustRotation (float delta) {
		rotationAngle += delta * rotationSpeed * Time.deltaTime;
		if (rotationAngle < 0f) {
			rotationAngle += 360f;
		}
		else if (rotationAngle >= 360f) {
			rotationAngle -= 360f;
		}
		transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
	}

	public void AdjustPosition (float xDelta, float zDelta) {
		Vector3 direction =
			transform.localRotation *
			new Vector3(xDelta, 0f, zDelta).normalized;
		float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
		float distance =
			Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) *
			damping * Time.deltaTime;

		Vector3 position = transform.localPosition;
		position += direction * distance;
		transform.localPosition =
			grid.wrapping ? WrapPosition(position) : ClampPosition(position);
	}

	public void CenterPostion()
	{
		Vector3 position = transform.localPosition;
		position.x = (grid.cellCountX - 0.5f) * HexMetrics.innerDiameter / 2;

		float offsetZ = Mathf.Sqrt(swivelMinZoom / 90f);

		position.z = (grid.cellCountZ - 1) * (1.5f * HexMetrics.outerRadius) / 2 * offsetZ;
		transform.localPosition =
			grid.wrapping ? WrapPosition(position) : ClampPosition(position);
	}


	Vector3 ClampPosition (Vector3 position) {
		//float xMin = offsetMoveCount * HexMetrics.innerDiameter;
		//float xMax = (grid.cellCountX - 0.5f) * HexMetrics.innerDiameter;
		//if (xMin > xMax / 2)
		//{
		//	xMax = xMax / 2;
		//}
		//position.x = Mathf.Clamp(position.x, xMin, xMax - xMin);
		
		//float zMax = (grid.cellCountZ - 1) * (1.5f * HexMetrics.outerRadius);
		//float offset = (xMax * Screen.height / Screen.width - zMax) / 2;
		//float zMin = xMin * Screen.height / Screen.width + offset;
		//if (zMin > zMax / 2)
		//{
		//	zMin = zMax / 2;
		//}
		//position.z = Mathf.Clamp(position.z, zMin, zMax - zMin);

		float xMax = (grid.cellCountX - 0.5f) * HexMetrics.innerDiameter;
		position.x = Mathf.Clamp(position.x, 0, xMax);

		float zMax = (grid.cellCountZ - 1) * (1.5f * HexMetrics.outerRadius);
		position.z = Mathf.Clamp(position.z, 0, zMax);
		return position;
	}

	Vector3 WrapPosition (Vector3 position) {
		float width = grid.cellCountX * HexMetrics.innerDiameter;
		while (position.x < 0f) {
			position.x += width;
		}
		while (position.x > width) {
			position.x -= width;
		}

		float zMax = (grid.cellCountZ - 1) * (1.5f * HexMetrics.outerRadius);
		position.z = Mathf.Clamp(position.z, 0f, zMax);

		grid.CenterMap(position.x);
		return position;
	}
}