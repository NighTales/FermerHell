public static class GameEvent
{
    public const string PAUSE = "PAUSE";
    public const string MAGIC_SHOOT = "MAGIC_SHOOT";
    public const string MUSIC_CHANGED = "MUSIC_CHANGED";
    public const string SOUNDS_CHANGED = "SOUNDS_CHANGED";
    public const string VOICE_CHANGED = "VOICE_CHANGED";
    public const string MOUSE_CHANGED = "MOUSE_CHANGED";
    public const string EXIT_LEVEL = "EXIT_LEVEL";

    public const string ENEMY_HIT = "ENEMY_HIT";
    public const string CHANGE_HEALTH = "CHANGE_HEALTH";
    public const string DAMAGE_MARKER_ACTIVATE = "DAMAGE_MARKER_ACTIVATE";
    public const string CHANGE_MAX_HEALTH = "CHANGE_MAX_HEALTH";
    public const string CHANGE_SPRINT_COUNT = "CHANGE_SPRINT_COUNT";
    public const string SPRINT_ACTION = "SPRINT_ACTION";
    public const string START_SPRINT = "START_SPRINT";
    public const string STOP_SPRINT = "STOP_SPRINT";
    public const string PLAYER_DEAD = "PLAYER_DEAD";
    public const string START_FINAL_LOADING = "START_FINAL_LOADING";


    public const string HIT = "HIT";
    public const string WEAPON_ARE_HIDDEN = "WEAPON_ARE_HIDDEN";
    public const string WEAPON_READY = "WEAPON_READY";
    public const string WEAPON_ARE_CHANGED = "WEAPON_ARE_CHANGED";
    public const string AMMO_ARE_CHANGED = "AMMO_HAS_CHANGED";
    public const string RETURN_TO_DEFAULT = "RETURN_TO_DEFAULT";

    public const string Set_BONUS_JUMP = "TAKE_BONUS_JUMP";
    public const string Set_BONUS_SPEED = "TAKE_BONUS_SPEED";
    public const string Set_BONUS_DOT = "TAKE_BONUS_DOT";
    public const string Set_BONUS_RESIST = "TAKE_BONUS_RESIST";
    public const string Set_BONUS_MAGNET = "TAKE_BONUS_MAGNET";
    
    public const string TAKE_BONUS_INVULNERABLE = "TAKE_BONUS_INVULNERABLE"; 
    
    public const string Set_DEBUFF_JUMP = "TAKE_DEBUFF_JUMP";
    public const string Set_DEBUFF_SPEED = "TAKE_DEBUFF_SPEED";
    public const string Set_DEBUFF_DOT = "TAKE_DEBUFF_DOT";
    public const string Set_DEBUFF_RESIST = "TAKE_DEBUFF_RESIST";
    public const string Set_DEBUFF_MAGNET = "TAKE_DEBUFF_MAGNET";

    public const string SET_R_BONUS = "SET_R_BONUS";
    public const string SET_B_BONUS = "SET_B_BONUS";
    public const string SET_G_BONUS = "SET_G_BONUS";

    public const string ADD_R_CHARGE = "ADD_R_CHARGE";
    public const string ADD_G_CHARGE = "ADD_G_CHARGE";
    public const string ADD_B_CHARGE = "ADD_B_CHARGE";

    public const string READY_TO_MAGIC = "READY_TO_MAGIC";
    public const string CLEAR_COLORS = "CLEAR_COLORS";
    
    
    public const string ENEMY_DEAD = "ENEMY_DEAD";
    public const string NEXT_WAVE = "NEXT_WAVE";
}