﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	const float speedMax = 10.0f;
	const float speedJump = 8.0f;
	const float speedAngle = 3.0f;
	Vector2  halfSizeScreen;

	Rigidbody RB;
	bool isJumping = false;
	Quaternion yaw = Quaternion.identity;
	float pitch = 0.0f;

	// Use this for initialization
	void Start () {
		this.RB = GetComponent<Rigidbody>();
		this.halfSizeScreen = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
	}

	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

//		this.RB.velocity = this.yaw * new Vector3(speedMax * h, this.RB.velocity.y, speedMax * v);
		this.RB.AddForce(this.yaw * new Vector3(speedMax * h, this.RB.velocity.y, speedMax * v) - this.RB.velocity, ForceMode.VelocityChange);
		if (!this.isJumping) {
//			bool doesJump = Input.GetButton("Jump");
			bool doesJump = Input.GetKeyDown(KeyCode.Space);
			if (doesJump) {
				this.isJumping = true;
				this.RB.AddForce(new Vector3(0.0f, 5.0f, 0.0f), ForceMode.VelocityChange);
			}
		}

		// マウスの位置が画面中央にある時は向きは変更しない。
		// 画面の上下端1/4のエリアにあるときに向きを変更
		var mousePos = Input.mousePosition;
//		Debug.Log("mosuePos=" + mousePos.ToString() + ", width=" + Screen.width.ToString() + ", height=" + Screen.height.ToString());
		float ratioH = (mousePos.x - halfSizeScreen.x) / halfSizeScreen.x;
		float ratioV = (mousePos.y - halfSizeScreen.y) / halfSizeScreen.y;
//		Debug.Log("ratioH = " + ratioH.ToString() + ", " + "ratioV = " + ratioV.ToString());
	
		// 左右の回転
		if (0.5f < Mathf.Abs(ratioH)) {
			this.yaw *= Quaternion.AngleAxis(speedAngle * (ratioH - Mathf.Sign(ratioH) * 0.5f), Vector3.up);
		}　
		// 上下の回転(プラスマイナス45度まで)
		if (0.5f < Mathf.Abs(ratioV)) {
			pitch += speedAngle * (ratioV - Mathf.Sign(ratioV) * 0.5f);
			if (pitch < -45.0f) {
				pitch = -45.0f;
			} else if (45.0f < pitch) {
				pitch = 45.0f;
			}
		}
		this.RB.MoveRotation(yaw * Quaternion.AngleAxis(pitch, Vector3.left));

	}
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("OnTriggerEnter(tag=" + other.tag + ")");
		if (other.CompareTag("Ground")) {
			this.isJumping = false;
		}
	}
}
