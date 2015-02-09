library IEEE;

use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity ProgramSynthesizedTemplate is
port( 
	clock : in std_logic;
	reset : in std_logic;
	RS232_dataout : out STD_LOGIC
);
end ProgramSynthesizedTemplate;

architecture Behavioral of ProgramSynthesizedTemplate is

	signal execution_started : std_logic;
	signal exec_done : std_logic := '0';
	signal call_1_ALU_Add_return_val : std_logic_vector(31 downto 0);
	signal call_1_ALU_Mul_return_val : std_logic_vector(31 downto 0);

	-- Serial transmission signals
	signal start_transmit_4bytes : std_logic := '0';	-- Flag to start transmission of the 4byte data
	signal send_4bytes_complete : std_logic := '0';		-- Flags if the transmission of a 4byte chucnk is complete
	signal data_4bytes : std_logic_vector(31 downto 0) := (others => '0'); -- 4 byte data to send 
	
	-- Signals here will be transmitted to a display device
	signal clockticks_elapsed : std_logic_vector(63 downto 0) := (others => '0');
	signal ms_elapsed : std_logic_vector(31 downto 0) := (others => '0');
	signal a : std_logic_vector(31 downto 0);
	signal b : std_logic_vector(31 downto 0);
	signal c : std_logic_vector(31 downto 0);
	signal d : std_logic_vector(31 downto 0);

	component ClockTimer is
		Port( 
				clock : in STD_LOGIC;
				reset : in STD_LOGIC;
				start : in STD_LOGIC;
				done : in STD_LOGIC;
				clockticks_elapsed : out STD_LOGIC_VECTOR(63 downto 0);
				ms_elapsed : out STD_LOGIC_VECTOR(31 downto 0)
			 );
	end component ClockTimer;
	
	component UartTransmit4 is
		Port( 
				clock : in STD_LOGIC;
				reset : in STD_LOGIC;
				start_transmit_4bytes: in STD_LOGIC;
				data_4bytes : in STD_LOGIC_VECTOR(31 downto 0);
				send_4bytes_complete : out STD_LOGIC;
				RS232_dataout : out STD_LOGIC
			 );
	end component UartTransmit4;

	component ALU_Add is
	port( 
		reset : in std_logic;
		op1 : in std_logic_vector(31 downto 0);
		op2 : in std_logic_vector(31 downto 0);
		result : out std_logic_vector(31 downto 0)
	);
	end component ALU_Add;

	component ALU_Mul is
	port( 
		reset : in std_logic;
		op1 : in std_logic_vector(31 downto 0);
		op2 : in std_logic_vector(31 downto 0);
		result : out std_logic_vector(31 downto 0)
	);
	end component ALU_Mul;


begin

	ms_timer : ClockTimer port map
	(
		clock => clock,
		reset => reset,
		start => not reset,
		done => exec_done,
		clockticks_elapsed => clockticks_elapsed,
		ms_elapsed => ms_elapsed
	);
	
	uart_send_4byte : UartTransmit4 port map
	(
		clock 						=> clock,
		reset 						=> reset,
		start_transmit_4bytes	=> start_transmit_4bytes,
		data_4bytes					=> data_4bytes,
		send_4bytes_complete  	=> send_4bytes_complete,
		RS232_dataout 				=> RS232_dataout
	);
	
	call_1_ALU_Add : ALU_Add port map
	(
		reset => reset,
		op1 => a,
		op2 => b,
		result => call_1_ALU_Add_return_val
	);
	
	call_1_ALU_Mul : ALU_Mul port map
	(
		reset => reset,
		op1 => c,
		op2 => b,
		result => call_1_ALU_Mul_return_val
	);

	proc_1_ProgramSynthesized : process(clock)
	begin
		ResetSync : if reset = '1' then
			execution_started <= '0';
			ms_elapsed <= X"00000000";
			
		elsif reset = '0' then
			ClockSync : if rising_edge(clock) then
				if execution_started = '0' then
					execution_started <= '1';
					a <= X"00000001";
					b <= X"00000002";
					
				end if ;
			end if ClockSync;
		end if ResetSync;
	end process proc_1_ProgramSynthesized;


	proc_2_call_1_ALU_Add_return_val : process(call_1_ALU_Add_return_val)
	begin
		ResetSync : if reset = '1' then

		elsif reset = '0' then
			c <= call_1_ALU_Add_return_val;
			
		end if ResetSync;
	end process proc_2_call_1_ALU_Add_return_val;


	proc_3_call_1_ALU_Mul_return_val : process(call_1_ALU_Mul_return_val)
	begin
		ResetSync : if reset = '1' then
			exec_done <= '0';
			
		elsif reset = '0' then
			d <= call_1_ALU_Mul_return_val;
			exec_done <= '1';
			
		end if ResetSync;
	end process proc_3_call_1_ALU_Mul_return_val;

end Behavioral;
