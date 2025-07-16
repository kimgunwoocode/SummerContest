
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int MaxHP; // �÷��̾� �ִ� ü��
    public int CurrentHP; // �÷��̾� ���� ü��
    public int ATK;
    public float MaxBreathGauge; // �ִ� �극�� ������
    public float CurrentBreathGauge; // �극�� ������
    public int Money; // �������� ��
    public List<int> EquipSkill; // �������� ��ų


    public Dictionary<int, bool> PlayerSkill = new(); // �رݵ� �÷��̾� ��ų <��ųID, �ر� ����>
    public Dictionary<int, bool> GettedItems = new(); // �������� ������
}