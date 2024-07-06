using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.InputField ifChangeCameraAt;
    [SerializeField] private UnityEngine.UI.InputField ifCameraIndex;
    [SerializeField] private UnityEngine.UI.Button btnChange;


    System.Action actCancel = null;

    // Start is called before the first frame update
    void Start()
    {
        btnChange.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
        btnChange.onClick.AddListener(OnClickChangeButton);

        // 全てのコンポーネントを停止.
        GameObject.FindObjectsOfType<UnityEngine.Behaviour>().ToList().ForEach(e=>e.enabled=false);

        // Startの実行順は同優先度上ではランダムだったはずなので、ここで全てを停止しても既にStartが実行されているコンポーネントも存在する事はあり得る.

        // 全てのコンポーネントの再開を予約実行.
        TimerAction.TimerSet(5.0f,()=>GameObject.FindObjectsOfType<UnityEngine.Behaviour>().ToList().ForEach(e=>e.enabled=true));

        // やり方によっては予約実行コンポーネントも止まってしまい,再開出来なくなる可能性があるので要注意.
        // 他のやり方でもやり方によってはコンポーネント始動の実行方法の場合、その管理コンポーネントを止めてしまうと復帰が出来なくなります.
        // その絡みやその他挙動が見えにくいので、普段僕は使わないですが、多分コルーチンもコルーチンを起動したオブジェクトを止めると止まってしまうんだった気がします.

        // 今回の場合、初めてTimerActionを起動する場所がここなので、
        // 
        // 　　全てのコンポーネントを停止
        //  → 予約
        //  → 予約処理内で予約実行コンポーネントを生成
        //  　 　※ 全てのコンポーネントを無効化した後に生成しているので無効化の対象から外れている.
        //  → 予約処理の実行
        //  → 全てのコンポーネントが復旧.
        //
        // と言う感じになってます.
        // 本来だと全てを無効化した後に本当に必要なものだけは有効にし直さないと多分ダメです.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickChangeButton()
    {
        // 実行済みの内容がある場合はキャンセルしておく.
        actCancel?.Invoke();

        // 処理オブジェクト実態.
        // 実行用オブジェクト内の処理で呼び出す時に必要に応じてポインタを外して多重実行抑止やキャンセル構造を実現する.
        System.Action actionEntity = 
            ()=>
            {
                // 実際にカメラを切り替える処理.
                CameraManager.Instance.SelectCamera(int.Parse(ifCameraIndex.text));

                // 多重実行を抑止.
                actionEntity = null;
            };

        // 処理オブジェクトキャンセル用処理オブジェクト.
        System.Action actionCancel =
            () =>
            {
                // キャンセル.
                actionEntity = null;
            };

        // 処理オブジェクト.
        // 実態の処理オブジェクトを間接的に実行するが、実行してはいけない場合は実態の処理オブジェクトのポインタを外して実態の処理オブジェクトを実行しないようにしてやる.
        System.Action action = 
            ()=>
            {
                // 実態の処理オブジェクトが存在している場合は実態処理を実行する.
                actionEntity?.Invoke();

                action = null;
            };

        // 予約実行.
        TimerAction.TimerSet(float.Parse(ifChangeCameraAt.text),action);
    }
}
