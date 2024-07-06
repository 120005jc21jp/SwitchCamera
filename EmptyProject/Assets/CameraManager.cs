using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // オブジェクトのメンバでは無くクラスキーワードのメンバとしてアクセスするためのキーワード、static.
    // public get; private set;でクラス内からはget,set出来て、クラス外からはgetのみ出来るようにする.
    public static CameraManager Instance { get; private set; }


    // インスペクタで参照を指定出来る形のカメラの集合.
    // インデックス参照で使用するカメラを指定する都合,
    // インデックスをずらしてしまうと,
    // Selectする際にどのインデックスを指定するとどのカメラを使う事になるのか分からなくなるので,
    // 仮に参照先のオブジェクトが破棄されて存在しないカメラになった場合でもそのアイテムを除去しない事にする.
    //  本当はIDを割り振って存在しなくなったオブジェクトは除去した方が良いが、実装が複雑になるので、取り敢えずやらない.
    [SerializeField] private List<Camera> cameras;


    // Start is called before the first frame update
    void Start()
    {
        // 既に別にカメラマネージャがある場合.
        if(Instance!=null)
        {
            // 自身を破壊.
            Destroy(this.gameObject);

            // この先の処理は実行しない.
            return;
        }

        Instance = this;

        // 初期化.
        // 必ずカメラは１個だけ使うと言うルールにあてはめておく.
        SelectCamera(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // カメラを登録する.
    // 動的に生成されるカメラなどがある場合に.
    public int RegisterCamera(Camera camera)
    {
        // カメラの集合にカメラを追加.
        cameras.Add(camera);

        // カメラをSelectする際の引数を返す.
        return cameras.Count-1;
    }


    // 使用するカメラを選択する.
    public void SelectCamera(int index)
    {
        // カメラの数でループする.
        for(int i=0; i<cameras.Count; ++i)
        {
            // 複数回呼ぶメンバの場合は参照をローカル変数に記録させる.
            Camera camera = cameras[i];

            // 集合にカメラ型のオブジェクトが入っている場合はGameObjectを取り出す.
            GameObject goCamera = camera!=null? camera.gameObject:null;

            // カメラ型からゲームオブジェクトを正しく取り出せた場合.
            if(goCamera!=null)
            {
                // インデックスに一致している場合はactive,それ以外はdisactive.
                goCamera.SetActive(i==index);
            }
        }
    }
}
