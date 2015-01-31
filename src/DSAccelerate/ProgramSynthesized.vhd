library IEEE;

use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity ProgramSynthesized is
port( 
	clock : in std_logic;
	reset : in std_logic;
	RS232_dataout : out STD_LOGIC
);
end ProgramSynthesized;

architecture Behavioral of ProgramSynthesized is

signal execution_started : std_logic;
signal call_1_ALU_Add_return_val : std_logic_vector(31 downto 0);
signal call_1_ALU_Mul_return_val : std_logic_vector(31 downto 0);
signal a : std_logic_vector(31 downto 0);
signal b : std_logic_vector(31 downto 0);
signal c : std_logic_vector(31 downto 0);
signal d : std_logic_vector(31 downto 0);

signal exec_done : std_logic := '0';

component UART_Transmitter is
	Port( clock : in STD_LOGIC;
			reset : in STD_LOGIC;
			send_data : in STD_LOGIC;
			dataout : out STD_LOGIC
		 );
end component UART_Transmitter;

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

uart_transmitdata : UART_Transmitter port map
(
	clock => clock,
	reset => reset,
	send_data => exec_done,
	dataout => RS232_dataout

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
