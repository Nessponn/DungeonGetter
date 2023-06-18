using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportMother : MonoBehaviour
{
    //�ŏ��̕�������m�[�h�����ŕ��������擾���Ă���

    public int Up_Length = 2;
    public int Down_Length = 2;
    public int Right_Length = 2;
    public int Left_Length = 2;


    //�����ԍ����i�[����I�u�W�F�N�g
    public GameObject RoomInfomations;
    public GameObject TresureRoomInfomations;//���󕔉����i�[����I�u�W�F�N�g

    public GameObject OriginRoom;//��ԍŏ��Ƀv���C���[���~�藧����

    private List<Room> Rooms = new List<Room>();//�ʏ핔���̏��
    private List<Room> TresureRooms = new List<Room>();//���󕔉��̏��
    private List<int> DecidedNumber = new List<int>();//���ɔz�u���ꂽ�����ԍ����i�[���邽�߂̔z��
    private List<int> TresureDecidedNumber = new List<int>();//���ɔz�u���ꂽ���󕔉����i�[���邽�߂̔z��

    // Start is called before the first frame update
    void Start()
    {
        //Rooms�z��ɁARoomInfomations���畔���̏����i�[����i�}�b�v���I�u�W�F�N�g�̎q�I�u�W�F�N�g�S���ۂ��ƂԂ����ށj
        for (int i =0; i < RoomInfomations.transform.childCount; i++)
        {
            Rooms.Add(RoomInfomations.transform.GetChild(i).GetComponent<Room>());
        }

        //TresureRoomInfomations�����l��
        for (int i = 0; i < TresureRoomInfomations.transform.childCount; i++)
        {
            TresureRooms.Add(TresureRoomInfomations.transform.GetChild(i).GetComponent<Room>());
        }

        //DecideNumber�z��ɁA�ԍ���U�蕪����
        for (int i = 0; i < Rooms.Count; i++)
        {
            DecidedNumber.Add(i);
        }

        //TresureDecideNumber�z��ɁA�ԍ���U�蕪����
        for (int i = 0; i < TresureRooms.Count; i++)
        {
            TresureDecidedNumber.Add(i);
        }
        //�󕔉��̐�
        int TresureRoom_Count = TresureDecidedNumber.Count;

        //�����̒��I���J�n
        RoomRourette_Up(OriginRoom.GetComponent<Room>(), Up_Length, 0);
        RoomRourette_Down(OriginRoom.GetComponent<Room>(), Down_Length, 0);
        RoomRourette_Right(OriginRoom.GetComponent<Room>(), Right_Length, 0);
        RoomRourette_Left(OriginRoom.GetComponent<Room>(), Left_Length, 0);

        //RoomRourette(OriginRoom.GetComponent<Room>(),2);

        //�����I����A�󔠂̎c�萔���V�X�e���ɓ`����
        PlayerStatus.Instance.Init(TresureRoom_Count -  TresureDecidedNumber.Count);
    }

    //(������t���������S�̕����A�c���)
    private void RoomRourette_Up(Room room, int Number, int Counts)
    {
        //�K��̉񐔈ȏ�ŏI��
        if (Number <= 0) return;

        //�����̏㕔�����߂�(�Q�[�g���ݒ肳��Ă���ꍇ)
        if (room.Up != null)
        {
            int DeadRock_Recovery = 0;//�f�b�h���b�N���p

            int num;
            int rand;
            Room WarpRoom;

            //�ʏ핔�������󕔉����𔻕ʂ���
            bool selectRoom;//true�Ȃ炨�󕔉��Afalse�Ȃ�ʏ핔��

            while (true)
            {
                //�����ԍ���I�o����
                //�m���ŁA�ʏ핔�������󕔉��������߂�
                //�������͊��ɁA�ʏ핔�������ׂĔz�u���I����Ă���ꍇ�����󕔉���I�o����
                if (DecidedNumber.Count <= 0) Debug.Log("�ʏ�@��ł��I");
                if (Random.Range(0, 10) <= Counts && TresureDecidedNumber.Count > 0)
                {
                    num = Random.Range(0, TresureDecidedNumber.Count);
                    rand = TresureDecidedNumber[num];

                    //�I�΂ꂽ�����ԍ������o���A�����ԍ��̕����𗘗p����
                    WarpRoom = TresureRooms[rand];

                    //�I�o�����̉��ɃQ�[�g������Ȃ�ʉ�
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

                    //�I�΂ꂽ�����ԍ������o���A�����ԍ��̕����𗘗p����
                    WarpRoom = Rooms[rand];

                    //�I�o�����̉��ɃQ�[�g������Ȃ�ʉ�
                    if (WarpRoom.Down != null)
                    {
                        selectRoom = false;
                        break;
                    }
                }
                //�Ȃ��Ȃ�Ē��I

                //���̎��_�łQ�O��ȏニ�[�v���Ă���ꍇ�͏����I��
                if (DeadRock_Recovery >= 20)
                {
                    Debug.Log("�����I��");
                    return;
                }
                DeadRock_Recovery++;
            }

            //�I�o���ꂽ�����̉����ƌq����
            room.Up.Warp = WarpRoom.Down;

            //�s�����ł���悤�Ɍq����
            WarpRoom.Down.Warp = room.Up;

            //�q�����̂ŗ��Q�[�g���A�����b�N��������
            room.Up.UnLock();
            WarpRoom.Down.UnLock();

            //�����Ƃ��Ēǉ������̂ŁA�I�o���ꂽ�����ԍ����폜
            if (selectRoom)
            {
                TresureDecidedNumber.RemoveAt(num);
                if (TresureDecidedNumber.Count <= 0) Debug.Log("����@��ł��I");
            }
            else
            {
                DecidedNumber.RemoveAt(num);
                if (DecidedNumber.Count <= 0) Debug.Log("�ʏ�@��ł��I");
            }




            //�㕔�̕������炳��Ɍ`������������
            //��Ȃ̂ŁA��A�E�A�����`��
            RoomRourette_Up(WarpRoom, Number - 1, Counts + 1);
            RoomRourette_Right(WarpRoom, Number - 1, Counts + 1);
            RoomRourette_Left(WarpRoom, Number - 1, Counts + 1);
        }
    }

    private void RoomRourette_Down(Room room, int Number, int Counts)
    {
        //�K��̉񐔈ȏ�ŏI��
        if (Number <= 0) return;


        //�����̉��������߂�(�Q�[�g���ݒ肳��Ă���ꍇ)
        if (room.Down != null)
        {
            int DeadRock_Recovery = 0;//�f�b�h���b�N���p

            int num;
            int rand;
            Room WarpRoom;

            //�ʏ핔�������󕔉����𔻕ʂ���
            bool selectRoom;//true�Ȃ炨�󕔉��Afalse�Ȃ�ʏ핔��

            while (true)
            {
                //�����ԍ���I�o����
                //�m���ŁA�ʏ핔�������󕔉��������߂�
                //�������͊��ɁA�ʏ핔�������ׂĔz�u���I����Ă���ꍇ�����󕔉���I�o����
                if (Random.Range(0, 10) <= Counts && TresureDecidedNumber.Count > 0)
                {
                    num = Random.Range(0, TresureDecidedNumber.Count);
                    rand = TresureDecidedNumber[num];

                    //�I�΂ꂽ�����ԍ������o���A�����ԍ��̕����𗘗p����
                    WarpRoom = TresureRooms[rand];

                    //�I�o�����̏�ɃQ�[�g������Ȃ�ʉ�
                    if (WarpRoom.Up != null)
                    {
                        selectRoom = true;
                        break;
                    }
                }
                else
                {
                    //�����ԍ���I�o����
                    num = Random.Range(0, DecidedNumber.Count);
                    rand = DecidedNumber[num];

                    //�I�΂ꂽ�����ԍ������o���A�����ԍ��̕����𗘗p����
                    WarpRoom = Rooms[rand];

                    //�I�o�����̏�ɃQ�[�g������Ȃ�ʉ�
                    if (WarpRoom.Up != null)
                    {
                        selectRoom = false;
                        break;
                    }

                }
                //�Ȃ��Ȃ�Ē��I

                //���̎��_�łQ�O��ȏニ�[�v���Ă���ꍇ�͏����I��
                if (DeadRock_Recovery >= 20) return;
                DeadRock_Recovery++;
            }

            //�I�o���ꂽ�����̏㑤�ƌq����
            room.Down.Warp = WarpRoom.Up;

            //�s�����ł���悤�Ɍq����
            WarpRoom.Up.Warp = room.Down;

            //�q�����̂ŗ��Q�[�g���A�����b�N��������
            room.Down.UnLock();
            WarpRoom.Up.UnLock();

            //�����Ƃ��Ēǉ������̂ŁA�I�o���ꂽ�����ԍ����폜
            if (selectRoom)
            {
                TresureDecidedNumber.RemoveAt(num);
                if (TresureDecidedNumber.Count <= 0) Debug.Log("����@��ł��I");
            }
            else
            {
                DecidedNumber.RemoveAt(num);
                if (DecidedNumber.Count <= 0) Debug.Log("�ʏ�@��ł��I");
            }



            //�����̕������炳��Ɍ`������������
            //���Ȃ̂ŁA���A�E�A�����`��
            RoomRourette_Down(WarpRoom, Number - 1, Counts + 1);
            RoomRourette_Right(WarpRoom, Number - 1, Counts + 1);
            RoomRourette_Left(WarpRoom, Number - 1, Counts + 1);
        }
    }

    private void RoomRourette_Right(Room room, int Number, int Counts)
    {
        //�K��̉񐔈ȏ�ŏI��
        if (Number <= 0) return;


        //�����̉E�������߂�(�Q�[�g���ݒ肳��Ă���ꍇ)
        if (room.Right != null)
        {
            int DeadRock_Recovery = 0;//�f�b�h���b�N���p

            int num;
            int rand;
            Room WarpRoom;

            //�ʏ핔�������󕔉����𔻕ʂ���
            bool selectRoom;//true�Ȃ炨�󕔉��Afalse�Ȃ�ʏ핔��

            while (true)
            {
                //�����ԍ���I�o����
                //�m���ŁA�ʏ핔�������󕔉��������߂�
                //�������͊��ɁA�ʏ핔�������ׂĔz�u���I����Ă���ꍇ�����󕔉���I�o����
                if (Random.Range(0, 10) <= Counts && TresureDecidedNumber.Count > 0)
                {
                    num = Random.Range(0, TresureDecidedNumber.Count);
                    rand = TresureDecidedNumber[num];

                    //�I�΂ꂽ�����ԍ������o���A�����ԍ��̕����𗘗p����
                    WarpRoom = TresureRooms[rand];

                    //�I�o�����̍��ɃQ�[�g������Ȃ�ʉ�
                    if (WarpRoom.Left != null)
                    {
                        selectRoom = true;
                        break;
                    }
                }
                else
                {
                    //�����ԍ���I�o����
                    num = Random.Range(0, DecidedNumber.Count);
                    rand = DecidedNumber[num];

                    //�I�΂ꂽ�����ԍ������o���A�����ԍ��̕����𗘗p����
                    WarpRoom = Rooms[rand];

                    //�I�o�����̍��ɃQ�[�g������Ȃ�ʉ�
                    if (WarpRoom.Left != null)
                    {
                        selectRoom = false;
                        break;
                    }

                }
                //�Ȃ��Ȃ�Ē��I

                //���̎��_�łQ�O��ȏニ�[�v���Ă���ꍇ�͏����I��
                if (DeadRock_Recovery >= 20) return;
                DeadRock_Recovery++;
            }

            //�I�o���ꂽ�����̍����ƌq����
            room.Right.Warp = WarpRoom.Left;

            //�s�����ł���悤�Ɍq����
            WarpRoom.Left.Warp = room.Right;

            //�q�����̂ŗ��Q�[�g���A�����b�N��������
            room.Right.UnLock();
            WarpRoom.Left.UnLock();

            //�����Ƃ��Ēǉ������̂ŁA�I�o���ꂽ�����ԍ����폜
            if (selectRoom)
            {
                TresureDecidedNumber.RemoveAt(num);
                if (TresureDecidedNumber.Count <= 0) Debug.Log("����@��ł��I");
            }
            else
            {
                DecidedNumber.RemoveAt(num);
                if (DecidedNumber.Count <= 0) Debug.Log("�ʏ�@��ł��I");
            }


            //�ǉ����������̏㕔�Ɖ����ɕ������Ȃ������ׂ�
            //����΂��ꂼ��Ȃ���
            Room MiningRoom;

            MiningRoom = room;

            //�㕔�T��
            //�L�΂����񐔕��A���̑O�ɍ��ɓ����Ă��炤�K�v����
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
                    //��i��ɏグ��
                    MiningRoom = MiningRoom.Up.Warp.room;

                    //������x�E�Ɍ����ă}�C�j���O
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
                        ////���̎��_�ŏ㑤�ɕ��������邱�Ƃ͊m��
                        //�����āA���̕����̉����ɃQ�[�g�������
                        //��������

                        //�ǉ����������ɂ͂���������ɃQ�[�g�����Ă��邩�̍ŏI�m�F
                        if (WarpRoom.Up != null)
                        {
                            //�������A�����͈͖��[�̕����Ōq���悤�Ƃ���ƁA���ꂪ�����邽�߁A
                            //���X�g�̐����ł͌q���Ȃ��悤�ɂ���
                            if (Number > 1)
                            {
                                //��������
                                WarpRoom.Up.Warp = MiningRoom.Down;
                                //�㑤�̕���������s�����ł���悤�Ȃ�
                                MiningRoom.Down.Warp = WarpRoom.Up;

                                //���݂��̃Q�[�g�A�����b�N����
                                WarpRoom.Up.UnLock();
                                MiningRoom.Down.UnLock();
                            }

                        }

                    }

                }
            }

            MiningRoom = room;


            //�����T��
            //�L�΂����񐔕��A���̑O�ɍ��ɓ����Ă��炤�K�v����
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
                    //��i���ɉ�����
                    MiningRoom = MiningRoom.Down.Warp.room;

                    //������x�E�Ɍ����ă}�C�j���O
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
                        ////���̎��_�ŉ����ɕ��������邱�Ƃ͊m��
                        //�����āA���̕����̏㑤�ɃQ�[�g�������
                        //��������

                        //�ǉ����������ɂ͂����������ɃQ�[�g�����Ă��邩�̍ŏI�m�F
                        if (WarpRoom.Down != null)
                        {
                            //�������A�����͈͖��[�̕����Ōq���悤�Ƃ���ƁA���ꂪ�����邽�߁A
                            //���X�g�̐����ł͌q���Ȃ��悤�ɂ���
                            if (Number > 1)
                            {
                                //��������
                                WarpRoom.Down.Warp = MiningRoom.Up;
                                //�㑤�̕���������s�����ł���悤�Ȃ�
                                MiningRoom.Up.Warp = WarpRoom.Down;

                                //���݂��̃Q�[�g�A�����b�N����
                                WarpRoom.Down.UnLock();
                                MiningRoom.Up.UnLock();
                            }
                        }
                    }
                }
            }
            //����ɉE�ցA�`���������s��
            RoomRourette_Right(WarpRoom, Number - 1, Counts + 1);
        }

    }

    private void RoomRourette_Left(Room room, int Number, int Counts)
    {
        //�K��̉񐔈ȏ�ŏI��
        if (Number <= 0) return;

        //�����̍��������߂�(�Q�[�g���ݒ肳��Ă���ꍇ)
        if (room.Left != null)
        {
            int DeadRock_Recovery = 0;//�f�b�h���b�N���p

            int num;
            int rand;
            Room WarpRoom;

            //�ʏ핔�������󕔉����𔻕ʂ���
            bool selectRoom;//true�Ȃ炨�󕔉��Afalse�Ȃ�ʏ핔��

            while (true)
            {
                //�����ԍ���I�o����
                //�m���ŁA�ʏ핔�������󕔉��������߂�
                //�������͊��ɁA�ʏ핔�������ׂĔz�u���I����Ă���ꍇ�����󕔉���I�o����
                if (Random.Range(0, 10) <= Counts && TresureDecidedNumber.Count > 0)
                {
                    num = Random.Range(0, TresureDecidedNumber.Count);
                    rand = TresureDecidedNumber[num];

                    //�I�΂ꂽ�����ԍ������o���A�����ԍ��̕����𗘗p����
                    WarpRoom = TresureRooms[rand];

                    //�I�o�����̏�ɃQ�[�g������Ȃ�ʉ�
                    if (WarpRoom.Right != null)
                    {
                        selectRoom = true;
                        break;
                    }
                }
                else
                {
                    //�����ԍ���I�o����
                    num = Random.Range(0, DecidedNumber.Count);
                    rand = DecidedNumber[num];

                    //�I�΂ꂽ�����ԍ������o���A�����ԍ��̕����𗘗p����
                    WarpRoom = Rooms[rand];

                    //�I�o�����̏�ɃQ�[�g������Ȃ�ʉ�
                    if (WarpRoom.Right != null)
                    {
                        selectRoom = false;
                        break;
                    }

                }
                //�Ȃ��Ȃ�Ē��I

                //���̎��_�łQ�O��ȏニ�[�v���Ă���ꍇ�͏����I��
                if (DeadRock_Recovery >= 20) return;
                DeadRock_Recovery++;
            }

            //�I�o���ꂽ�����̍����ƌq����
            room.Left.Warp = WarpRoom.Right;

            //�s�����ł���悤�Ɍq����
            WarpRoom.Right.Warp = room.Left;

            //�q�����̂ŗ��Q�[�g���A�����b�N��������
            room.Left.UnLock();
            WarpRoom.Right.UnLock();

            //�����Ƃ��Ēǉ������̂ŁA�I�o���ꂽ�����ԍ����폜
            if (selectRoom)
            {
                TresureDecidedNumber.RemoveAt(num);
                if (TresureDecidedNumber.Count <= 0) Debug.Log("����@��ł��I");
            }
            else
            {
                DecidedNumber.RemoveAt(num);
                if (DecidedNumber.Count <= 0) Debug.Log("�ʏ�@��ł��I");
            }



            //�ǉ����������̏㕔�Ɖ����ɕ������Ȃ������ׂ�
            //����΂��ꂼ��Ȃ���
            Room MiningRoom;

            MiningRoom = room;

            //�㕔�T��
            //�L�΂����񐔕��A���̑O�ɉE�ɓ����Ă��炤�K�v����
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
                    //��i��ɏグ��
                    MiningRoom = MiningRoom.Up.Warp.room;

                    //������x���Ɍ����ă}�C�j���O
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

                        ////���̎��_�ŏ㑤�ɕ��������邱�Ƃ͊m��
                        //�����āA���̕����̉����ɃQ�[�g�������
                        //��������

                        //�ǉ����������ɂ͂���������ɃQ�[�g�����Ă��邩�̍ŏI�m�F
                        if (WarpRoom.Up != null)
                        {
                            //�������A�����͈͖��[�̕����Ōq���悤�Ƃ���ƁA���ꂪ�����邽�߁A
                            //���X�g�̐����ł͌q���Ȃ��悤�ɂ���
                            if (Number > 1)
                            {

                                //��������
                                WarpRoom.Up.Warp = MiningRoom.Down;
                                //�㑤�̕���������s�����ł���悤�Ȃ�
                                MiningRoom.Down.Warp = WarpRoom.Up;

                                //���݂��̃Q�[�g�A�����b�N����
                                WarpRoom.Up.UnLock();
                                MiningRoom.Down.UnLock();
                            }

                        }

                    }

                }
            }
            
            MiningRoom = room;


            //�����T��
            //�L�΂����񐔕��A���̑O�ɉE�ɓ����Ă��炤�K�v����
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
                    //��i���ɉ�����
                    MiningRoom = MiningRoom.Down.Warp.room;

                    //������x���Ɍ����ă}�C�j���O
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

                        ////���̎��_�ŉ����ɕ��������邱�Ƃ͊m��
                        //�����āA���̕����̏㑤�ɃQ�[�g�������
                        //��������

                        //�ǉ����������ɂ͂����������ɃQ�[�g�����Ă��邩�̍ŏI�m�F
                        if (WarpRoom.Down != null)
                        {
                            //�������A�����͈͖��[�̕����Ōq���悤�Ƃ���ƁA���ꂪ�����邽�߁A
                            //���X�g�̐����ł͌q���Ȃ��悤�ɂ���
                            if(Number > 1)
                            {

                                //��������
                                WarpRoom.Down.Warp = MiningRoom.Up;
                                //�㑤�̕���������s�����ł���悤�Ȃ�
                                MiningRoom.Up.Warp = WarpRoom.Down;

                                //���݂��̃Q�[�g�A�����b�N����
                                WarpRoom.Down.UnLock();
                                MiningRoom.Up.UnLock();
                            }
                        }
                    }
                }
            }
            //����ɍ��ցA�`���������s��
            RoomRourette_Left(WarpRoom, Number - 1, Counts + 1);
        }
    }
}

public static class ListExtensions
{
    /// <summary>
    /// �擪�ɂ���I�u�W�F�N�g���폜�����ɕԂ��܂�
    /// </summary>
    public static T Peek<T>(this IList<T> self)
    {
        return self[0];
    }

    /// <summary>
    /// �擪�ɂ���I�u�W�F�N�g���폜���A�Ԃ��܂�
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
    /// �����ɃI�u�W�F�N�g��ǉ����܂�
    /// </summary>
    public static void Push<T>(this IList<T> self, T item)
    {
        self.Insert(0, item);
    }
}
