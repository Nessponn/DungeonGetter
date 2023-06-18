using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportMother : MonoBehaviour
{
    //最初の部屋からノード方式で部屋情報を取得していく

    public int Up_Length = 2;
    public int Down_Length = 2;
    public int Right_Length = 2;
    public int Left_Length = 2;


    //部屋番号を格納するオブジェクト
    public GameObject RoomInfomations;
    public GameObject TresureRoomInfomations;//お宝部屋を格納するオブジェクト

    public GameObject OriginRoom;//一番最初にプレイヤーが降り立つ部屋

    private List<Room> Rooms = new List<Room>();//通常部屋の情報
    private List<Room> TresureRooms = new List<Room>();//お宝部屋の情報
    private List<int> DecidedNumber = new List<int>();//既に配置された部屋番号を格納するための配列
    private List<int> TresureDecidedNumber = new List<int>();//既に配置されたお宝部屋を格納するための配列

    // Start is called before the first frame update
    void Start()
    {
        //Rooms配列に、RoomInfomationsから部屋の情報を格納する（マップ情報オブジェクトの子オブジェクト郡を丸ごとぶち込む）
        for (int i =0; i < RoomInfomations.transform.childCount; i++)
        {
            Rooms.Add(RoomInfomations.transform.GetChild(i).GetComponent<Room>());
        }

        //TresureRoomInfomationsも同様に
        for (int i = 0; i < TresureRoomInfomations.transform.childCount; i++)
        {
            TresureRooms.Add(TresureRoomInfomations.transform.GetChild(i).GetComponent<Room>());
        }

        //DecideNumber配列に、番号を振り分ける
        for (int i = 0; i < Rooms.Count; i++)
        {
            DecidedNumber.Add(i);
        }

        //TresureDecideNumber配列に、番号を振り分ける
        for (int i = 0; i < TresureRooms.Count; i++)
        {
            TresureDecidedNumber.Add(i);
        }
        //宝部屋の数
        int TresureRoom_Count = TresureDecidedNumber.Count;

        //部屋の抽選を開始
        RoomRourette_Up(OriginRoom.GetComponent<Room>(), Up_Length, 0);
        RoomRourette_Down(OriginRoom.GetComponent<Room>(), Down_Length, 0);
        RoomRourette_Right(OriginRoom.GetComponent<Room>(), Right_Length, 0);
        RoomRourette_Left(OriginRoom.GetComponent<Room>(), Left_Length, 0);

        //RoomRourette(OriginRoom.GetComponent<Room>(),2);

        //生成終了後、宝箱の残り数をシステムに伝える
        PlayerStatus.Instance.Init(TresureRoom_Count -  TresureDecidedNumber.Count);
    }

    //(部屋を付け足す中心の部屋、残り回数)
    private void RoomRourette_Up(Room room, int Number, int Counts)
    {
        //規定の回数以上で終了
        if (Number <= 0) return;

        //部屋の上部を決める(ゲートが設定されている場合)
        if (room.Up != null)
        {
            int DeadRock_Recovery = 0;//デッドロック回避用

            int num;
            int rand;
            Room WarpRoom;

            //通常部屋かお宝部屋かを判別する
            bool selectRoom;//trueならお宝部屋、falseなら通常部屋

            while (true)
            {
                //部屋番号を選出する
                //確率で、通常部屋かお宝部屋かを決める
                //もしくは既に、通常部屋をすべて配置し終わっている場合もお宝部屋を選出する
                if (DecidedNumber.Count <= 0) Debug.Log("通常　空です！");
                if (Random.Range(0, 10) <= Counts && TresureDecidedNumber.Count > 0)
                {
                    num = Random.Range(0, TresureDecidedNumber.Count);
                    rand = TresureDecidedNumber[num];

                    //選ばれた部屋番号を取り出し、部屋番号の部屋を利用する
                    WarpRoom = TresureRooms[rand];

                    //選出部屋の下にゲートがあるなら通過
                    if (WarpRoom.Down != null)
                    {
                        selectRoom = true;
                        break;
                    }
                }
                else
                {
                    num = Random.Range(0, DecidedNumber.Count);
                    rand = DecidedNumber[num];

                    //選ばれた部屋番号を取り出し、部屋番号の部屋を利用する
                    WarpRoom = Rooms[rand];

                    //選出部屋の下にゲートがあるなら通過
                    if (WarpRoom.Down != null)
                    {
                        selectRoom = false;
                        break;
                    }
                }
                //ないなら再抽選

                //この時点で２０回以上ループしている場合は処理終了
                if (DeadRock_Recovery >= 20)
                {
                    Debug.Log("処理終了");
                    return;
                }
                DeadRock_Recovery++;
            }

            //選出された部屋の下側と繋げる
            room.Up.Warp = WarpRoom.Down;

            //行き来できるように繋げる
            WarpRoom.Down.Warp = room.Up;

            //繋げたので両ゲートをアンロック処理する
            room.Up.UnLock();
            WarpRoom.Down.UnLock();

            //部屋として追加したので、選出された部屋番号を削除
            if (selectRoom)
            {
                TresureDecidedNumber.RemoveAt(num);
                if (TresureDecidedNumber.Count <= 0) Debug.Log("お宝　空です！");
            }
            else
            {
                DecidedNumber.RemoveAt(num);
                if (DecidedNumber.Count <= 0) Debug.Log("通常　空です！");
            }




            //上部の部屋からさらに形成処理をする
            //上なので、上、右、左を形成
            RoomRourette_Up(WarpRoom, Number - 1, Counts + 1);
            RoomRourette_Right(WarpRoom, Number - 1, Counts + 1);
            RoomRourette_Left(WarpRoom, Number - 1, Counts + 1);
        }
    }

    private void RoomRourette_Down(Room room, int Number, int Counts)
    {
        //規定の回数以上で終了
        if (Number <= 0) return;


        //部屋の下部を決める(ゲートが設定されている場合)
        if (room.Down != null)
        {
            int DeadRock_Recovery = 0;//デッドロック回避用

            int num;
            int rand;
            Room WarpRoom;

            //通常部屋かお宝部屋かを判別する
            bool selectRoom;//trueならお宝部屋、falseなら通常部屋

            while (true)
            {
                //部屋番号を選出する
                //確率で、通常部屋かお宝部屋かを決める
                //もしくは既に、通常部屋をすべて配置し終わっている場合もお宝部屋を選出する
                if (Random.Range(0, 10) <= Counts && TresureDecidedNumber.Count > 0)
                {
                    num = Random.Range(0, TresureDecidedNumber.Count);
                    rand = TresureDecidedNumber[num];

                    //選ばれた部屋番号を取り出し、部屋番号の部屋を利用する
                    WarpRoom = TresureRooms[rand];

                    //選出部屋の上にゲートがあるなら通過
                    if (WarpRoom.Up != null)
                    {
                        selectRoom = true;
                        break;
                    }
                }
                else
                {
                    //部屋番号を選出する
                    num = Random.Range(0, DecidedNumber.Count);
                    rand = DecidedNumber[num];

                    //選ばれた部屋番号を取り出し、部屋番号の部屋を利用する
                    WarpRoom = Rooms[rand];

                    //選出部屋の上にゲートがあるなら通過
                    if (WarpRoom.Up != null)
                    {
                        selectRoom = false;
                        break;
                    }

                }
                //ないなら再抽選

                //この時点で２０回以上ループしている場合は処理終了
                if (DeadRock_Recovery >= 20) return;
                DeadRock_Recovery++;
            }

            //選出された部屋の上側と繋げる
            room.Down.Warp = WarpRoom.Up;

            //行き来できるように繋げる
            WarpRoom.Up.Warp = room.Down;

            //繋げたので両ゲートをアンロック処理する
            room.Down.UnLock();
            WarpRoom.Up.UnLock();

            //部屋として追加したので、選出された部屋番号を削除
            if (selectRoom)
            {
                TresureDecidedNumber.RemoveAt(num);
                if (TresureDecidedNumber.Count <= 0) Debug.Log("お宝　空です！");
            }
            else
            {
                DecidedNumber.RemoveAt(num);
                if (DecidedNumber.Count <= 0) Debug.Log("通常　空です！");
            }



            //下部の部屋からさらに形成処理をする
            //下なので、下、右、左を形成
            RoomRourette_Down(WarpRoom, Number - 1, Counts + 1);
            RoomRourette_Right(WarpRoom, Number - 1, Counts + 1);
            RoomRourette_Left(WarpRoom, Number - 1, Counts + 1);
        }
    }

    private void RoomRourette_Right(Room room, int Number, int Counts)
    {
        //規定の回数以上で終了
        if (Number <= 0) return;


        //部屋の右部を決める(ゲートが設定されている場合)
        if (room.Right != null)
        {
            int DeadRock_Recovery = 0;//デッドロック回避用

            int num;
            int rand;
            Room WarpRoom;

            //通常部屋かお宝部屋かを判別する
            bool selectRoom;//trueならお宝部屋、falseなら通常部屋

            while (true)
            {
                //部屋番号を選出する
                //確率で、通常部屋かお宝部屋かを決める
                //もしくは既に、通常部屋をすべて配置し終わっている場合もお宝部屋を選出する
                if (Random.Range(0, 10) <= Counts && TresureDecidedNumber.Count > 0)
                {
                    num = Random.Range(0, TresureDecidedNumber.Count);
                    rand = TresureDecidedNumber[num];

                    //選ばれた部屋番号を取り出し、部屋番号の部屋を利用する
                    WarpRoom = TresureRooms[rand];

                    //選出部屋の左にゲートがあるなら通過
                    if (WarpRoom.Left != null)
                    {
                        selectRoom = true;
                        break;
                    }
                }
                else
                {
                    //部屋番号を選出する
                    num = Random.Range(0, DecidedNumber.Count);
                    rand = DecidedNumber[num];

                    //選ばれた部屋番号を取り出し、部屋番号の部屋を利用する
                    WarpRoom = Rooms[rand];

                    //選出部屋の左にゲートがあるなら通過
                    if (WarpRoom.Left != null)
                    {
                        selectRoom = false;
                        break;
                    }

                }
                //ないなら再抽選

                //この時点で２０回以上ループしている場合は処理終了
                if (DeadRock_Recovery >= 20) return;
                DeadRock_Recovery++;
            }

            //選出された部屋の左側と繋げる
            room.Right.Warp = WarpRoom.Left;

            //行き来できるように繋げる
            WarpRoom.Left.Warp = room.Right;

            //繋げたので両ゲートをアンロック処理する
            room.Right.UnLock();
            WarpRoom.Left.UnLock();

            //部屋として追加したので、選出された部屋番号を削除
            if (selectRoom)
            {
                TresureDecidedNumber.RemoveAt(num);
                if (TresureDecidedNumber.Count <= 0) Debug.Log("お宝　空です！");
            }
            else
            {
                DecidedNumber.RemoveAt(num);
                if (DecidedNumber.Count <= 0) Debug.Log("通常　空です！");
            }


            //追加した部屋の上部と下部に部屋がないか調べる
            //あればそれぞれつなげる
            Room MiningRoom;

            MiningRoom = room;

            //上部探索
            //伸ばした回数分、この前に左に動いてもらう必要あり
            for (int i = 0; i < Counts; i++)
            {
                if (MiningRoom.Left != null)
                {
                    if (MiningRoom.Left.Warp != null)
                    {
                        MiningRoom = MiningRoom.Left.Warp.room;
                    }
                }
            }
            if (MiningRoom.Up != null)
            {
                if (MiningRoom.Up.Warp != null)
                {
                    //一段上に上げる
                    MiningRoom = MiningRoom.Up.Warp.room;

                    //もう一度右に向けてマイニング
                    for (int i = 0; i <= Counts; i++)
                    {
                        if (MiningRoom.Right != null)
                        {
                            if (MiningRoom.Right.Warp != null)
                            {
                                MiningRoom = MiningRoom.Right.Warp.room;
                            }
                        }
                    }

                    if (MiningRoom.Down != null)
                    {
                        ////この時点で上側に部屋があることは確定
                        //続いて、その部屋の下側にゲートがあれば
                        //結合する

                        //追加した部屋にはそもそも上にゲートがついているかの最終確認
                        if (WarpRoom.Up != null)
                        {
                            //ただし、生成範囲末端の部屋で繋げようとすると、ずれが生じるため、
                            //ラストの生成では繋げないようにする
                            if (Number > 1)
                            {
                                //結合処理
                                WarpRoom.Up.Warp = MiningRoom.Down;
                                //上側の部屋からも行き来できるようつなぐ
                                MiningRoom.Down.Warp = WarpRoom.Up;

                                //お互いのゲートアンロック処理
                                WarpRoom.Up.UnLock();
                                MiningRoom.Down.UnLock();
                            }

                        }

                    }

                }
            }

            MiningRoom = room;


            //下部探索
            //伸ばした回数分、この前に左に動いてもらう必要あり
            for (int i = 0; i < Counts; i++)
            {
                if (MiningRoom.Left != null)
                {
                    if (MiningRoom.Left.Warp != null)
                    {
                        MiningRoom = MiningRoom.Left.Warp.room;
                    }
                }
            }
            if (MiningRoom.Down != null)
            {
                if (MiningRoom.Down.Warp != null)
                {
                    //一段下に下げる
                    MiningRoom = MiningRoom.Down.Warp.room;

                    //もう一度右に向けてマイニング
                    for (int i = 0; i <= Counts; i++)
                    {
                        if (MiningRoom.Right != null)
                        {
                            if (MiningRoom.Right.Warp != null)
                            {
                                MiningRoom = MiningRoom.Right.Warp.room;
                            }
                        }
                    }

                    if (MiningRoom.Up != null)
                    {
                        ////この時点で下側に部屋があることは確定
                        //続いて、その部屋の上側にゲートがあれば
                        //結合する

                        //追加した部屋にはそもそも下にゲートがついているかの最終確認
                        if (WarpRoom.Down != null)
                        {
                            //ただし、生成範囲末端の部屋で繋げようとすると、ずれが生じるため、
                            //ラストの生成では繋げないようにする
                            if (Number > 1)
                            {
                                //結合処理
                                WarpRoom.Down.Warp = MiningRoom.Up;
                                //上側の部屋からも行き来できるようつなぐ
                                MiningRoom.Up.Warp = WarpRoom.Down;

                                //お互いのゲートアンロック処理
                                WarpRoom.Down.UnLock();
                                MiningRoom.Up.UnLock();
                            }
                        }
                    }
                }
            }
            //さらに右へ、形成処理を行う
            RoomRourette_Right(WarpRoom, Number - 1, Counts + 1);
        }

    }

    private void RoomRourette_Left(Room room, int Number, int Counts)
    {
        //規定の回数以上で終了
        if (Number <= 0) return;

        //部屋の左部を決める(ゲートが設定されている場合)
        if (room.Left != null)
        {
            int DeadRock_Recovery = 0;//デッドロック回避用

            int num;
            int rand;
            Room WarpRoom;

            //通常部屋かお宝部屋かを判別する
            bool selectRoom;//trueならお宝部屋、falseなら通常部屋

            while (true)
            {
                //部屋番号を選出する
                //確率で、通常部屋かお宝部屋かを決める
                //もしくは既に、通常部屋をすべて配置し終わっている場合もお宝部屋を選出する
                if (Random.Range(0, 10) <= Counts && TresureDecidedNumber.Count > 0)
                {
                    num = Random.Range(0, TresureDecidedNumber.Count);
                    rand = TresureDecidedNumber[num];

                    //選ばれた部屋番号を取り出し、部屋番号の部屋を利用する
                    WarpRoom = TresureRooms[rand];

                    //選出部屋の上にゲートがあるなら通過
                    if (WarpRoom.Right != null)
                    {
                        selectRoom = true;
                        break;
                    }
                }
                else
                {
                    //部屋番号を選出する
                    num = Random.Range(0, DecidedNumber.Count);
                    rand = DecidedNumber[num];

                    //選ばれた部屋番号を取り出し、部屋番号の部屋を利用する
                    WarpRoom = Rooms[rand];

                    //選出部屋の上にゲートがあるなら通過
                    if (WarpRoom.Right != null)
                    {
                        selectRoom = false;
                        break;
                    }

                }
                //ないなら再抽選

                //この時点で２０回以上ループしている場合は処理終了
                if (DeadRock_Recovery >= 20) return;
                DeadRock_Recovery++;
            }

            //選出された部屋の左側と繋げる
            room.Left.Warp = WarpRoom.Right;

            //行き来できるように繋げる
            WarpRoom.Right.Warp = room.Left;

            //繋げたので両ゲートをアンロック処理する
            room.Left.UnLock();
            WarpRoom.Right.UnLock();

            //部屋として追加したので、選出された部屋番号を削除
            if (selectRoom)
            {
                TresureDecidedNumber.RemoveAt(num);
                if (TresureDecidedNumber.Count <= 0) Debug.Log("お宝　空です！");
            }
            else
            {
                DecidedNumber.RemoveAt(num);
                if (DecidedNumber.Count <= 0) Debug.Log("通常　空です！");
            }



            //追加した部屋の上部と下部に部屋がないか調べる
            //あればそれぞれつなげる
            Room MiningRoom;

            MiningRoom = room;

            //上部探索
            //伸ばした回数分、この前に右に動いてもらう必要あり
            for (int i = 0; i < Counts; i++)
            {
                if (MiningRoom.Right != null)
                {
                    if (MiningRoom.Right.Warp != null)
                    {
                        MiningRoom = MiningRoom.Right.Warp.room;
                    }
                }
            }
            if (MiningRoom.Up != null)
            {
                if (MiningRoom.Up.Warp != null)
                {
                    //一段上に上げる
                    MiningRoom = MiningRoom.Up.Warp.room;

                    //もう一度左に向けてマイニング
                    for (int i = 0; i <= Counts; i++)
                    {
                        if (MiningRoom.Left != null)
                        {
                            if (MiningRoom.Left.Warp != null)
                            {
                                MiningRoom = MiningRoom.Left.Warp.room;
                            }
                        }
                    }

                    if (MiningRoom.Down != null)
                    {

                        ////この時点で上側に部屋があることは確定
                        //続いて、その部屋の下側にゲートがあれば
                        //結合する

                        //追加した部屋にはそもそも上にゲートがついているかの最終確認
                        if (WarpRoom.Up != null)
                        {
                            //ただし、生成範囲末端の部屋で繋げようとすると、ずれが生じるため、
                            //ラストの生成では繋げないようにする
                            if (Number > 1)
                            {

                                //結合処理
                                WarpRoom.Up.Warp = MiningRoom.Down;
                                //上側の部屋からも行き来できるようつなぐ
                                MiningRoom.Down.Warp = WarpRoom.Up;

                                //お互いのゲートアンロック処理
                                WarpRoom.Up.UnLock();
                                MiningRoom.Down.UnLock();
                            }

                        }

                    }

                }
            }
            
            MiningRoom = room;


            //下部探索
            //伸ばした回数分、この前に右に動いてもらう必要あり
            for (int i = 0; i < Counts; i++)
            {
                if (MiningRoom.Right != null)
                {
                    if (MiningRoom.Right.Warp != null)
                    {
                        MiningRoom = MiningRoom.Right.Warp.room;
                    }
                }
            }
            if(MiningRoom.Down != null)
            {
                if (MiningRoom.Down.Warp != null)
                {
                    //一段下に下げる
                    MiningRoom = MiningRoom.Down.Warp.room;

                    //もう一度左に向けてマイニング
                    for (int i = 0; i <= Counts; i++)
                    {
                        if (MiningRoom.Left != null)
                        {
                            if (MiningRoom.Left.Warp != null)
                            {
                                MiningRoom = MiningRoom.Left.Warp.room;
                            }
                        }
                    }

                    if(MiningRoom.Up != null)
                    {

                        ////この時点で下側に部屋があることは確定
                        //続いて、その部屋の上側にゲートがあれば
                        //結合する

                        //追加した部屋にはそもそも下にゲートがついているかの最終確認
                        if (WarpRoom.Down != null)
                        {
                            //ただし、生成範囲末端の部屋で繋げようとすると、ずれが生じるため、
                            //ラストの生成では繋げないようにする
                            if(Number > 1)
                            {

                                //結合処理
                                WarpRoom.Down.Warp = MiningRoom.Up;
                                //上側の部屋からも行き来できるようつなぐ
                                MiningRoom.Up.Warp = WarpRoom.Down;

                                //お互いのゲートアンロック処理
                                WarpRoom.Down.UnLock();
                                MiningRoom.Up.UnLock();
                            }
                        }
                    }
                }
            }
            //さらに左へ、形成処理を行う
            RoomRourette_Left(WarpRoom, Number - 1, Counts + 1);
        }
    }
}

public static class ListExtensions
{
    /// <summary>
    /// 先頭にあるオブジェクトを削除せずに返します
    /// </summary>
    public static T Peek<T>(this IList<T> self)
    {
        return self[0];
    }

    /// <summary>
    /// 先頭にあるオブジェクトを削除し、返します
    /// </summary>
    public static T Pop<T>(this IList<T> self)
    {
        var result = self[0];
        self.RemoveAt(0);
        return result;
    }
    public static T Pop<T>(this IList<T> self,int num)
    {
        var result = self[num];
        self.RemoveAt(num);
        return result;
    }
    /// <summary>
    /// 末尾にオブジェクトを追加します
    /// </summary>
    public static void Push<T>(this IList<T> self, T item)
    {
        self.Insert(0, item);
    }
}
