using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { Sword, Range, Guard, Wizard, Bullet };
public enum Condition { Move, Attack, Hit, Die, Stop};

public class Unit : MonoBehaviour
{
	public UnitType type; //������ Ÿ��
	public int speed; 
	public ParticleSystem Dust;

	private float distance; //������ ����
	LayerMask enemy; //������ ���̾�
	LayerMask ally; //�Ʊ��� ���̾�
	RaycastHit2D hit; 
	RaycastHit2D wait;
	Animator anim;
	Vector3 direction; //���� Ž�� �������� �� ����
	Vector3 allyDirection; // �Ʊ� Ž�� �������� �� ����
	private Condition con; //������ �� ����
	private bool enemyScan = true; 




	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	private void Start()
	{
		//���� Ÿ�� ���� �ʿ��� �� ����
		TypeSet();
		// ù ����� �̵����� ����
		con = Condition.Move;
		anim.SetBool("doMove", true);
	}

	private void Update()
	{
		doMove();
	}

	private void FixedUpdate()
	{
		if (con == Condition.Move) //�̵� ������ ��
		{
			if (enemyScan) // ���� Ž�� ���� true �϶�
			{
				hit = Physics2D.Raycast(transform.position, direction, distance, enemy); //���� Ž���ϴ� ������ 
				Debug.DrawRay(transform.position, direction, Color.green, 0f);
				if (hit)
				{
					con = Condition.Attack; //���� Attack���� �ٲ�
					doAttack(); //����
				}
				else // Ž�� ���н� ���� Ž�� false
					enemyScan = false;
			}
			else //���� Ž�� false�� ��
			{
				wait = Physics2D.Raycast(transform.position, allyDirection, 0.3f, ally);//�Ʊ� Ž�� �������� ���� Ÿ�Կ� ���� ��ȭ���� ����
				Debug.DrawRay(transform.position, allyDirection, Color.blue, 0f);
				if (wait)
				{
					con = Condition.Stop;//Ž�� �Ǹ� Stop����
					doStop();
					Debug.Log(con);
				}
				else //�Ʊ� Ž�� ���н� ���� Ž�� true
					enemyScan = true; 
			}
		}
	}

	private void doMove()
	{
		if (con == Condition.Move) //�̵� ������ �� ����Ʈ ��ƼŬ ���� �� �̵�
		{
			if (!(Dust == null))
				Dust.Play();
			transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, 0f, 0f);
		}
		else//�̵� ���� �ƴҶ� ��ƼŬ ���� ����
		{
			if (!(Dust == null))
				Dust.Stop();
		}

	}

	private void TypeSet()
	{
		switch (type) // ���� Ÿ�Ժ��� ���� Ž�� ������ ���� ����
		{
			case UnitType.Sword:
				distance = 0.7f;
				break;
			case UnitType.Range:
				distance = 2.5f;
				break;
			case UnitType.Guard:
				distance = 0.7f;
				break;
			case UnitType.Wizard:
				distance = 3.5f;
				break;
			case UnitType.Bullet:
				distance = 0.4f;
				break;
		}
		//�� ������ ���� ���� ���� ����
		if (gameObject.layer == 8)
		{
			ally = LayerMask.GetMask("Blue");
			enemy = LayerMask.GetMask("Red");
			direction = Vector3.right * distance;
			allyDirection = Vector3.right * 0.3f;
		}
		else if (gameObject.layer == 9)
		{
			ally = LayerMask.GetMask("Red");
			enemy = LayerMask.GetMask("Blue");
			direction = Vector3.left * distance;
			allyDirection = Vector3.left * 0.3f;
		}
	}

	private void doAttack()
	{
		if (con == Condition.Attack)
		{
			anim.SetBool("doAttack", true);
			StartCoroutine(AttackDelay());
		}
	}

	IEnumerator AttackDelay()
	{
		yield return new WaitForSeconds(Random.Range(0.8f, 1.2f));
		con = Condition.Move;
		anim.SetBool("doMove", true);
	}

	private void doStop()
	{
		if (con == Condition.Stop)
		{
			StartCoroutine(StopDelay());
		}

	}
	IEnumerator StopDelay()
	{
		yield return new WaitForSeconds(2f);
		con = Condition.Move;
		anim.SetBool("doMove", true);
	}
}
