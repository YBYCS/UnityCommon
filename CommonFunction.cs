
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
        /// ��z������תһ������
        /// </summary>
        /// <param name="original"></param>
        /// <param name="angle"></param>
        /// <returns>����һ����ת�������</returns>
        public static Vector2 TurnV2(Vector2 original, float angle) {
            if (original == Vector2.zero) return Vector2.zero;
            angle *= Mathf.PI / 180;
            float mag = original.magnitude;
            angle += Mathf.Atan2(original.y, original.x);
            return new Vector2(mag * Mathf.Cos(angle), mag * Mathf.Sin(angle));
        }
        /// <summary>
        /// ����һ���ı��ļ�,����ΪUTF8,���û�ж�Ӧ���ļ����򴴽���Ӧ���ļ���
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="data">Ҫд�������</param>
        /// <param name="fileMode">д��ģʽ</param>
        public static void FileIO(string path, string data, FileMode fileMode = FileMode.Create) {
            FileInfo fileInfo = new FileInfo(path);
            if (File.Exists(path) && fileMode == FileMode.Create) {
                Debug.Log("ע��,�Ѵ���:" + path + "���ļ����ᱻ����");
            }
            var d = fileInfo.Directory;
            if (!d.Exists) {
                Debug.Log("�ļ�Ŀ¼" + d + "������,���Զ�����");
                d.Create();
            }
            using FileStream file = new(path, FileMode.Create);
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            file.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// �׳�һ���쳣
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
        /// move the go when the (awsd)key down. you don't need hundle the input. ���Ƽ�ʹ�ô˺���
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="speed"></param>
        /// <param name="isOverLook">�Ƿ��Ǹ��ӽǶ�</param>
        public static void MoveGameObject(GameObject gameObject, float speed, bool isOverLook) {
            Vector3 dir = Input.GetAxisRaw("Horizontal") * gameObject.transform.right;

            if (isOverLook) {
                dir += Input.GetAxisRaw("Virtual") * gameObject.transform.up;
            }

            gameObject.transform.Translate(dir * Time.deltaTime * speed, Space.World);
        }

        /// <summary>
        /// ��ô�����ļнǶ���
        /// </summary>
        /// <param name="fromVector"></param>
        /// <param name="toVector"></param>
        /// <returns></returns>
        public static float GetV3Angle(Vector3 fromVector, Vector3 toVector) {
            float angle = Vector3.Angle(fromVector, toVector); //���������֮��ļн�
            Vector3 normal = Vector3.Cross(fromVector, toVector);//��������������
            angle *= Mathf.Sign(Vector3.Dot(normal, Vector3.up)); //���������������Ϸ���������ˣ����Ϊ1��-1��������ת����
            return angle;
        }

        /// <summary>
        /// Check if it is on the ground. The gameObject must have collider2d
        /// layermask�������õ�һЩ�ܽ᣺1 << 10 �򿪵�10�Ĳ㡣~(1 << 10) �򿪳��˵�10֮��Ĳ㡣~(1 << 0) �����еĲ㡣(1 << 10) | (1 << 8) �򿪵�10�͵�8�Ĳ㡣
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
        /// �����ݴ�ӡ
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
        /// �ӽ���Ļʱ��Ե�ƶ������λ�ã������Update������ִ��
        /// </summary>
        /// <param name="MoveCamera">Ҫ�ƶ����������Ĭ��Ϊ�������</param>
        /// <param name="targetPosition">��������λ�ã�Ĭ��Ϊ���λ�ã�����ο�ϵΪ��Ļ���زο�ϵ</param>
        /// <param name="speed">�ƶ��ٶ�</param>
        /// <param name="distance">������ƶ�����</param>
        /// <param name="borderThickness">�����</param>
        public static void MoveCameraWhenCloseBorder( Vector3 targetPosition,Transform MoveCamera = default,
            float speed = 5.0f, float distance = 10.0f,float borderThickness = 10.0f, float minX = -float.MaxValue,
            float minY = -float.MaxValue, float maxX = float.MaxValue, float maxY = float.MaxValue) {
            if (MoveCamera == default) {
                MoveCamera = Camera.main.transform;
            }
            // ��ȡ��Ļ�ߴ�
            Vector3 screenSize = new Vector3(Screen.width, Screen.height, 0);
            // ������������Ļ��Ե�ľ���
            Vector3 targetDistanceFromEdge = targetPosition - screenSize / 2;

            // ������ӽ���Ļ��Ե���ƶ������
            if (Mathf.Abs(targetDistanceFromEdge.x) > screenSize.x / 2 - borderThickness) {
                float direction = Mathf.Sign(targetDistanceFromEdge.x);
                MoveCamera.Translate(new Vector3(direction * speed * Time.deltaTime, 0, 0));
            }
            if (Mathf.Abs(targetDistanceFromEdge.y) > screenSize.y / 2 - borderThickness) {
                float direction = Mathf.Sign(targetDistanceFromEdge.y);
                MoveCamera.Translate(new Vector3(0, direction * speed * Time.deltaTime, 0));
            }

            //�޶���Χ
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
        /// ����Ŀ�������ƶ����ƶ���Ļ
        /// </summary>
        /// <param name="lastPosition">һ���洢��һ��λ�õ���������Ҫ��ʹ���ߴ���</param>
        /// <param name="condition">�ƶ�������������Input.GetMouseButton(1)</param>
        /// <param name="targetPosition">Ŀ�����ڵ�λ��,����Input.mousePosition</param>
        /// <param name="speed">�ƶ��ٶ�</param>
        /// <param name="MoveCamera">�ƶ������</param>
        /// <param name="direction">�ƶ��ķ���Ĭ��Ϊ�෴</param>
        /// <param name="minX">��Ļ�ƶ������Ʒ�Χ</param>
        /// <param name="minY">��Ļ�ƶ������Ʒ�Χ</param>
        /// <param name="maxX">��Ļ�ƶ������Ʒ�Χ</param>
        /// <param name="maxY">��Ļ�ƶ������Ʒ�Χ</param>
        public static void MoveCameraViaMoveTarget(ref Vector3 lastPosition, bool condition, Vector3 targetPosition,
            float speed = 5f, Transform MoveCamera = default, int direction = -1, float minX = -float.MaxValue,
            float minY = -float.MaxValue, float maxX = float.MaxValue, float maxY = float.MaxValue) {
            // Check if the right mouse button is being held down
            if (condition) {
                if (MoveCamera == default) {
                    MoveCamera = Camera.main.transform;
                }

                //ƫ����
                Vector3 delta = targetPosition - lastPosition;

                lastPosition = targetPosition;

                //������λ��
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
        /// ��ȡ������ڵ��������겢������z
        /// </summary>
        /// <param name="z"></param>
        public static Vector3 GetMouseWorldPositionAndSetZ(float z=0) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = z;
            return mousePos;
        }

        /// <summary>
        /// ���������������
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




