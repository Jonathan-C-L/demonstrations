typedef enum PortJ_PullTypedef_{
    PortJ_PullUp = 1, 
    PortJ_PullDown = 0
} PortJ_Pull;

typedef enum PortJ_EdgeTypedef_{
    PortJ_RisingEdge = 1,
    PortJ_FallingEdge = 0
} PortJ_Edge;

typedef enum PortJ_InterruptTypedef_{
    PortJ_IEnable = 1,
    PortJ_IDisable = 0
} PortJ_Interrupt;

typedef enum PortJ_IOTypedef_{
    PortJ_Input = 0,
    PortJ_Output = 1
} PortJ_IO;
typedef enum PortJ_ButtonTypedef_{
    PortJ_J0 = 1U,
    PortJ_J1 = 2U
} PortJ_Button;


/// @brief Configures Port J
/// @param bt Specifies the specific button to configure
/// @param pull Port J can either be setup with a pull up (normally up) or pull down (normally low) 
/// @param edge Port J can be activated on the rising edge (button pressed) or falling edge (button released)
/// @param enable Enable on the Port J interrupt 
void PortJ_InitInputButton(PortJ_Button bt, PortJ_Pull pull, PortJ_Edge edge, PortJ_Interrupt enable);

/// @brief Adds a callback function to a specified PortJ Function
/// @param bt Specified button to add a callback to
/// @param pFunction a function pointer that will be used within the interrupt (if enabled)
void PortJ_AddButtonCallback(PortJ_Button bt, void (*pFunction)(void));