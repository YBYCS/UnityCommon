
using LYcommon;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;


public static class CommonFunction
{
    /// <summary>
    /// ��z������תһ������
    /// </summary>
    /// <param name="original"></param>
    /// <param name="angle"></param>
    /// <returns>����һ����ת�������</returns>
    public static Vector2 TurnV2(Vector2 original, float angle)
    {
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
    public static void FileIO(string path, string data,  FileMode fileMode = FileMode.Create)
    {
        FileInfo fileInfo = new FileInfo(path);
        if (File.Exists(path)&&fileMode == FileMode.Create)
        {
            Debug.Log("ע��,�Ѵ���:" + path + "���ļ����ᱻ����");
        }
        var d = fileInfo.Directory;
        if (!d.Exists)
        {
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
    public static void ThrowException(string data)
    {
        throw new CustomException(data);
    }

    /// <summary>
    /// get the hit under the mouse in 2d
    /// </summary>
    /// <returns>return the hit, it may be null. You can get collider by using hit.collider</returns>
    public static RaycastHit2D GetHitUnderTheMouse2D()
    {
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
    public static void MoveGameObject(GameObject gameObject, float speed, bool isOverLook)
    {
        Vector3 dir = Input.GetAxisRaw("Horizontal") * gameObject.transform.right;

        if (isOverLook)
        {
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
    public static float GetV3Angle(Vector3 fromVector, Vector3 toVector)
    {
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
    public static bool isOnGround2D(GameObject gameObject,int layerMash, float extraDistance =0.1f)
    {
        var hit = Physics2D.Raycast(gameObject.transform.position, -gameObject.transform.up, extraDistance + gameObject.GetComponent<Collider2D>().bounds.extents.y, layerMash);
        return hit.collider != null;
    }

}


