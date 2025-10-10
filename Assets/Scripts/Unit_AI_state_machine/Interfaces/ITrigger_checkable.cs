using UnityEngine;

public interface ITrigger_checkable
{
    bool is_seeing_opponent { get; set; }

    bool is_opponent_in_attack_range { get; set; }
    
    void set_is_seeing_opponent_status(bool _is_seeing_opponent);
    void set_is_opponent_in_attack_range_bool(bool _is_opponent_in_attack_range);
}
