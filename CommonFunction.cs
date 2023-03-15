
using LYcommon;
using System.IO;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Transform = UnityEngine.Transform;

namespace LYcommon {
    /// <summary>
    /// Some Common Function
    /// </summary>
    public class CommonFunction {
        /// <summary>
        /// 绕z轴逆旋转一定度数
        /// </summary>
        /// <param name="original"></param>
        /// <param name="angle"></param>
        /// <returns>返回一个旋转后的向量</returns>
        public static Vector2 TurnV2(Vector2 original, float angle) {
            if (original == Vector2.zero) return Vector2.zero;
            angle *= Mathf.PI / 180;
            float mag = original.magnitude;
            angle += Mathf.Atan2(original.y, original.x);
            return new Vector2(mag * Mathf.Cos(angle), mag * Mathf.Sin(angle));
        }
        /// <summary>
        /// 创建一个文本文件,编码为UTF8,如果没有对应的文件夹则创建对应的文件夹
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="data">要写入的数据</param>
        /// <param name="fileMode">写入模式</param>
        public static void FileIO(string path, string data, FileMode fileMode = FileMode.Create) {
            FileInfo fileInfo = new FileInfo(path);
            if (File.Exists(path) && fileMode == FileMode.Create) {
                Debug.Log("注意,已存在:" + path + "该文件将会被覆盖");
            }
            var d = fileInfo.Directory;
            if (!d.Exists) {
                Debug.Log("文件目录" + d + "不存在,已自动创建");
                d.Create();
            }
            using FileStream file = new(path, FileMode.Create);
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            file.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 抛出一个异常
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="CustomException"></exception>
        public static void ThrowException(string data) {
            throw new CustomException(data);
        }

        /// <summary>
        /// get the hit under the mouse in 2d
        /// </summary>
        /// <returns>return the hit, it may be null. You can get collider by using hit.collider</returns>
        public static RaycastHit2D GetHitUnderTheMouse2D() {
            var onScreenPosition = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(onScreenPosition);
            var hit = Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y), Vector2.zero, Mathf.Infinity);
            return hit;
        }

        /// <summary>
        /// move the go when the (awsd)key down. you don't need hundle the input. 不推荐使用此函数
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="speed"></param>
        /// <param name="isOverLook">是否是俯视角度</param>
        public static void MoveGameObject(GameObject gameObject, float speed, bool isOverLook) {
            Vector3 dir = Input.GetAxisRaw("Horizontal") * gameObject.transform.right;

            if (isOverLook) {
                dir += Input.GetAxisRaw("Virtual") * gameObject.transform.up;
            }

            gameObject.transform.Translate(dir * Time.deltaTime * speed, Space.World);
        }

        /// <summary>
        /// 获得带方向的夹角度数
        /// </summary>
        /// <param name="fromVector"></param>
        /// <param name="toVector"></param>
        /// <returns></returns>
        public static float GetV3Angle(Vector3 fromVector, Vector3 toVector) {
            float angle = Vector3.Angle(fromVector, toVector); //求出两向量之间的夹角
            Vector3 normal = Vector3.Cross(fromVector, toVector);//叉乘求出法线向量
            angle *= Mathf.Sign(Vector3.Dot(normal, Vector3.up)); //求法线向量与物体上方向向量点乘，结果为1或-1，修正旋转方向
            return angle;
        }

        /// <summary>
        /// Check if it is on the ground. The gameObject must have collider2d
        /// layermask参数设置的一些总结：1 << 10 打开第10的层。~(1 << 10) 打开除了第10之外的层。~(1 << 0) 打开所有的层。(1 << 10) | (1 << 8) 打开第10和第8的层。
        /// </summary>
        /// <param name="gameObject">object to be detected</param>
        /// <param name="layerMash">the layerMesh. If you want deceted the ground whose layer is 7, you should pass parameter "1<<7"</param>
        /// <param name="extraDistance">The distance to be deteted in addition to the distance to itself</param>
        /// <returns></returns>
        public static bool isOnGround2D(GameObject gameObject, int layerMash, float extraDistance = 0.1f) {
            var hit = Physics2D.Raycast(gameObject.transform.position, -gameObject.transform.up, extraDistance + gameObject.GetComponent<Collider2D>().bounds.extents.y, layerMash);
            return hit.collider != null;
        }
        /// <summary>
        /// 多内容打印
        /// </summary>
        public static void DebugMultiple(params object[] log) {
            string s = "";
            foreach (var obj in log) {
                //if (obj is int || obj is float || obj is double || obj is decimal) {
                //    s += obj + " ";
                //}
                //else {
                //    s += obj.ToString() + " ";
                //}
                s += obj.ToString() + "   ";
            }
            Debug.Log(s);
        }

        public static float GetXOnBezierCurve(Vector2 startPoint, Vector2 controlPoint1, Vector2 controlPoint2,
            Vector2 endPoint, float y) {
            // Calculate the x coordinates of the four points
            float x1 = startPoint.x;
            float x2 = controlPoint1.x;
            float x3 = controlPoint2.x;
            float x4 = endPoint.x;

            // Use binary search to find t, such that the y coordinate of the point on the curve is y
            float t = 0.5f; // start with t=0.5
            float y0 = 0;
            float error = 0.01f; // the maximum allowed error
            float step = 0.25f; // the step size for binary search
            while (Mathf.Abs(y0 - y) > error) {
                y0 = Mathf.Pow(1 - t, 3) * startPoint.y + 3 * t * Mathf.Pow(1 - t, 2) * controlPoint1.y + 3 * Mathf.Pow(t, 2) * (1 - t) * controlPoint2.y + Mathf.Pow(t, 3) * endPoint.y;
                if (y0 < y) {
                    t += step;
                }
                else {
                    t -= step;
                }
                step /= 2; // reduce the step size
            }

            // Return the x coordinate of the point on the curve at the found t
            return Mathf.Pow(1 - t, 3) * x1 + 3 * t * Mathf.Pow(1 - t, 2) * x2 + 3 * Mathf.Pow(t, 2) * (1 - t) * x3 + Mathf.Pow(t, 3) * x4;
        }
        /// <summary>
        /// 接近屏幕时边缘移动摄像机位置，请放在Update函数中执行
        /// </summary>
        /// <param name="MoveCamera">要移动的摄像机，默认为主摄像机</param>
        /// <param name="targetPosition">检测物体的位置，默认为鼠标位置，坐标参考系为屏幕像素参考系</param>
        /// <param name="speed">移动速度</param>
        /// <param name="distance">摄像机移动距离</param>
        /// <param name="borderThickness">检测厚度</param>
        public static void MoveCameraWhenCloseBorder( Vector3 targetPosition,Transform MoveCamera = default,
            float speed = 5.0f, float distance = 10.0f,float borderThickness = 10.0f, float minX = -float.MaxValue,
            float minY = -float.MaxValue, float maxX = float.MaxValue, float maxY = float.MaxValue) {
            if (MoveCamera == default) {
                MoveCamera = Camera.main.transform;
            }
            // 获取屏幕尺寸
            Vector3 screenSize = new Vector3(Screen.width, Screen.height, 0);
            // 计算鼠标距离屏幕边缘的距离
            Vector3 targetDistanceFromEdge = targetPosition - screenSize / 2;

            // 如果鼠标接近屏幕边缘，移动摄像机
            if (Mathf.Abs(targetDistanceFromEdge.x) > screenSize.x / 2 - borderThickness) {
                float direction = Mathf.Sign(targetDistanceFromEdge.x);
                MoveCamera.Translate(new Vector3(direction * speed * Time.deltaTime, 0, 0));
            }
            if (Mathf.Abs(targetDistanceFromEdge.y) > screenSize.y / 2 - borderThickness) {
                float direction = Mathf.Sign(targetDistanceFromEdge.y);
                MoveCamera.Translate(new Vector3(0, direction * speed * Time.deltaTime, 0));
            }

            //限定范围
            if (MoveCamera.position.x < minX) {
                MoveCamera.position = new(minX, MoveCamera.position.y, MoveCamera.position.z);
            }

            if (MoveCamera.position.y < minY) {
                MoveCamera.position = new(MoveCamera.position.x, minY, MoveCamera.position.z);
            }


            if (MoveCamera.position.x > maxX) {
                MoveCamera.position = new(maxX, MoveCamera.position.y, MoveCamera.position.z);
            }



            if (MoveCamera.position.y > maxY) {
                MoveCamera.position = new(MoveCamera.position.x, maxY, MoveCamera.position.z);
            }
        }
        /// <summary>
        /// 根据目标的相对移动，移动屏幕
        /// </summary>
        /// <param name="lastPosition">一个存储上一次位置的向量，需要由使用者储存</param>
        /// <param name="condition">移动的条件，例如Input.GetMouseButton(1)</param>
        /// <param name="targetPosition">目标现在的位置,例如Input.mousePosition</param>
        /// <param name="speed">移动速度</param>
        /// <param name="MoveCamera">移动的相机</param>
        /// <param name="direction">移动的方向，默认为相反</param>
        /// <param name="minX">屏幕移动的限制范围</param>
        /// <param name="minY">屏幕移动的限制范围</param>
        /// <param name="maxX">屏幕移动的限制范围</param>
        /// <param name="maxY">屏幕移动的限制范围</param>
        public static void MoveCameraViaMoveTarget(ref Vector3 lastPosition, bool condition, Vector3 targetPosition,
            float speed = 5f, Transform MoveCamera = default, int direction = -1, float minX = -float.MaxValue,
            float minY = -float.MaxValue, float maxX = float.MaxValue, float maxY = float.MaxValue) {
            // Check if the right mouse button is being held down
            if (condition) {
                if (MoveCamera == default) {
                    MoveCamera = Camera.main.transform;
                }

                //偏移量
                Vector3 delta = targetPosition - lastPosition;

                lastPosition = targetPosition;

                //计算新位置
                Vector3 newPosition = MoveCamera.position;
                newPosition += direction * MoveCamera.right * delta.x * speed * Time.deltaTime;
                newPosition += direction * MoveCamera.up * delta.y * speed * Time.deltaTime;

                MoveCamera.position = newPosition;


                if (MoveCamera.position.x < minX) {
                    MoveCamera.position = new(minX, MoveCamera.position.y, MoveCamera.position.z);
                }

                if (MoveCamera.position.y < minY) {
                    MoveCamera.position = new(MoveCamera.position.x, minY, MoveCamera.position.z);
                }


                if (MoveCamera.position.x > maxX) {
                    MoveCamera.position = new(maxX, MoveCamera.position.y, MoveCamera.position.z);
                }



                if (MoveCamera.position.y > maxY) {
                    MoveCamera.position = new(MoveCamera.position.x, maxY, MoveCamera.position.z);
                }

            }
            else {
                lastPosition = targetPosition;
            }
        }
        /// <summary>
        /// 获取鼠标现在的世界坐标并且设置z
        /// </summary>
        /// <param name="z"></param>
        public static Vector3 GetMouseWorldPositionAndSetZ(float z=0) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = z;
            return mousePos;
        }

        /// <summary>
        /// 滚轮缩放主摄像机
        /// </summary>
        /// <param name="zoomSpeed"></param>
        /// <param name="minZoom"></param>
        /// <param name="maxZoom"></param>
        public static void ZoomCamera(float zoomSpeed = 1,float minZoom = 1,float maxZoom = 10f) {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float size = Mathf.Clamp(Camera.main.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
            Camera.main.orthographicSize = size;
        }
    }

}




