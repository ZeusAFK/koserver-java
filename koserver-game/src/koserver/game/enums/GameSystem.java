package koserver.game.enums;

public class GameSystem {

    public static final int KARUS_ID = 1;
    public static final int ELMORAD_ID = 2;

    public static final int MAX_ACCOUNT_CHARACTER = 3;  // Maximun characters per account

    // Item slot define
    public static final short RIGHTEAR = 0;
    public static final short HEAD = 1;
    public static final short LEFTEAR = 2;
    public static final short NECK = 3;
    public static final short BREAST = 4;
    public static final short SHOULDER = 5;
    public static final short RIGHTHAND = 6;
    public static final short WAIST = 7;
    public static final short LEFTHAND = 8;
    public static final short RIGHTRING = 9;
    public static final short LEG = 10;
    public static final short LEFTRING = 11;
    public static final short GLOVE = 12;
    public static final short FOOT = 13;
    public static final short RESERVED = 14;

    // Item slots size define
    public static final short EQUIPED_SLOTS = 14;       // Equiped item slots
    public static final short INVENTORY_SLOTS = 28;     // Inventory slots
    public static final short COSPRE_SLOTS = 5;         // Costume premium slots (1 Wing, 2 Gloves, 1 Helmet, 1 Armor)
    public static final short MAGICBAG_COUNT = 2;       // Magic bag count
    public static final short MAGICBAG_SLOTS = 12;      // Magic bag slots

    public static final short MAGICBAG_TOTAL_SLOTS = MAGICBAG_COUNT * MAGICBAG_SLOTS;
    public static final short INVENTORY_AREA = EQUIPED_SLOTS;
    public static final short COSPRE_AREA = INVENTORY_AREA + INVENTORY_SLOTS;
    public static final short MAGICBAGS_AREA = COSPRE_AREA + COSPRE_SLOTS;
    public static final short MAGICBAG1_AREA = MAGICBAGS_AREA + MAGICBAG_COUNT;
    public static final short MAGICBAG2_AREA = MAGICBAG1_AREA + MAGICBAG_SLOTS;
    public static final short INVENTARY_TOTAL_SLOTS = MAGICBAG2_AREA + MAGICBAG_SLOTS;
}
