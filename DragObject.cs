using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LYcommon {
    /// <summary>
    /// 使用鼠标拖拽物体
    /// </summary>
    public class DragObject : MonoBehaviour {

        private Vector3 offset;

        private void OnMouseDown() {
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        }

        private void OnMouseDrag() {
            Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            transform.position = Camera.main.ScreenToWorldPoint(newPosition) + offset;
        }
    }
}

