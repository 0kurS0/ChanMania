using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public static ChunkGenerator m_instance;
    
    public Chunk[] m_chunks;
    public Chunk m_firstChunk;
    public Camera m_playerCamera;
    [HideInInspector]
    public int m_chunksSpawned = 0;

    private List<Chunk> _spawnedChunks = new List<Chunk>();

    private void Awake() {
        if(m_instance == null)
            m_instance = this;
    }

    private void Start() {
        _spawnedChunks.Add(m_firstChunk);
    }

    public void SpawnChunk(){
        m_chunksSpawned++;

        Chunk _newChunk = Instantiate(m_chunks[Random.Range(0, m_chunks.Length)]);
        _newChunk.transform.position = _spawnedChunks[_spawnedChunks.Count - 1].m_end.position - _newChunk.m_begin.localPosition;
        _spawnedChunks.Add(_newChunk);

        if(_spawnedChunks.Count >= 5){
            Destroy(_spawnedChunks[0].gameObject);
            _spawnedChunks.RemoveAt(0);
        }
        //Change background
        if(m_chunksSpawned == 1){
            m_playerCamera.backgroundColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            m_chunksSpawned=0;
        }
    }
}
