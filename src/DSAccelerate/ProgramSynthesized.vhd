library IEEE;

use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity ProgramSynthesized is
port( 
	clock : in std_logic;
	reset : in std_logic
);
end ProgramSynthesized;

architecture Behavioral of ProgramSynthesized is

signal execution_started : std_logic;
signal call_1_Increment_return_val : std_logic_vector(31 downto 0);
signal call_1_ALU_Add_return_val : std_logic_vector(31 downto 0);
signal call_1_ALU_Sub_return_val : std_logic_vector(31 downto 0);
signal call_1_ALU_Mul_return_val : std_logic_vector(31 downto 0);
signal call_1_ALU_Div_return_val : std_logic_vector(31 downto 0);
signal x : std_logic_vector(31 downto 0);
signal tSSA_4_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal tSSA_5_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal y : std_logic_vector(31 downto 0);
signal a : std_logic_vector(31 downto 0);
signal b : std_logic_vector(31 downto 0);
signal tSSA_6_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal tSSA_7_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal tSSA_8_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal c : std_logic_vector(31 downto 0);
signal tSSA_9_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal tSSA_10_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal tSSA_11_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal d : std_logic_vector(31 downto 0);
signal tSSA_12_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal tSSA_13_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal tSSA_14_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal e : std_logic_vector(31 downto 0);
signal tSSA_15_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal tSSA_16_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal tSSA_17_4cc42779139d4e269b03bb7d3686bb3c : std_logic_vector(31 downto 0);
signal f : std_logic_vector(31 downto 0);

component Increment is
port( 
	reset : in std_logic;
	i : in std_logic_vector(31 downto 0);
	return_Increment : out std_logic_vector(31 downto 0)
);
end component Increment;
component ALU_Add is
port( 
	reset : in std_logic;
	op1 : in std_logic_vector(31 downto 0);
	op2 : in std_logic_vector(31 downto 0);
	result : out std_logic_vector(31 downto 0)
);
end component ALU_Add;
component ALU_Sub is
port( 
	reset : in std_logic;
	op1 : in std_logic_vector(31 downto 0);
	op2 : in std_logic_vector(31 downto 0);
	result : out std_logic_vector(31 downto 0)
);
end component ALU_Sub;
component ALU_Mul is
port( 
	reset : in std_logic;
	op1 : in std_logic_vector(31 downto 0);
	op2 : in std_logic_vector(31 downto 0);
	result : out std_logic_vector(31 downto 0)
);
end component ALU_Mul;
component ALU_Div is
port( 
	reset : in std_logic;
	op1 : in std_logic_vector(31 downto 0);
	op2 : in std_logic_vector(31 downto 0);
	result : out std_logic_vector(31 downto 0)
);
end component ALU_Div;

begin
call_1_Increment : Increment port map
(
reset => reset,
i => tSSA_4_4cc42779139d4e269b03bb7d3686bb3c,
return_Increment => call_1_Increment_return_val
);
call_1_ALU_Add : ALU_Add port map
(
reset => reset,
op1 => tSSA_6_4cc42779139d4e269b03bb7d3686bb3c,
op2 => tSSA_7_4cc42779139d4e269b03bb7d3686bb3c,
result => call_1_ALU_Add_return_val
);
call_1_ALU_Sub : ALU_Sub port map
(
reset => reset,
op1 => tSSA_9_4cc42779139d4e269b03bb7d3686bb3c,
op2 => tSSA_10_4cc42779139d4e269b03bb7d3686bb3c,
result => call_1_ALU_Sub_return_val
);
call_1_ALU_Mul : ALU_Mul port map
(
reset => reset,
op1 => tSSA_12_4cc42779139d4e269b03bb7d3686bb3c,
op2 => tSSA_13_4cc42779139d4e269b03bb7d3686bb3c,
result => call_1_ALU_Mul_return_val
);
call_1_ALU_Div : ALU_Div port map
(
reset => reset,
op1 => tSSA_15_4cc42779139d4e269b03bb7d3686bb3c,
op2 => tSSA_16_4cc42779139d4e269b03bb7d3686bb3c,
result => call_1_ALU_Div_return_val
);

proc_1_ProgramSynthesized : process(clock)

begin
ResetSync : if reset = '1' then
execution_started <= '0';

elsif reset = '0' then
ClockSync : if rising_edge(clock) then
if execution_started = '0' then
execution_started <= '1';
x <= X"00000001";

end if ;

end if ClockSync;
end if ResetSync;


end process proc_1_ProgramSynthesized;

proc_2_x : process(x)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
tSSA_4_4cc42779139d4e269b03bb7d3686bb3c <= x;
end if ResetSync;


end process proc_2_x;

proc_3_call_1_Increment_return_val : process(call_1_Increment_return_val)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
tSSA_5_4cc42779139d4e269b03bb7d3686bb3c <= call_1_Increment_return_val;
end if ResetSync;


end process proc_3_call_1_Increment_return_val;

proc_4_tSSA_5_4cc42779139d4e269b03bb7d3686bb3c : process(tSSA_5_4cc42779139d4e269b03bb7d3686bb3c)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
y <= tSSA_5_4cc42779139d4e269b03bb7d3686bb3c;
a <= X"0000000A";
b <= X"00000005";
end if ResetSync;


end process proc_4_tSSA_5_4cc42779139d4e269b03bb7d3686bb3c;

proc_5_a : process(a)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
tSSA_6_4cc42779139d4e269b03bb7d3686bb3c <= a;
tSSA_7_4cc42779139d4e269b03bb7d3686bb3c <= b;
end if ResetSync;


end process proc_5_a;

proc_6_call_1_ALU_Add_return_val : process(call_1_ALU_Add_return_val)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
tSSA_8_4cc42779139d4e269b03bb7d3686bb3c <= call_1_ALU_Add_return_val;
end if ResetSync;


end process proc_6_call_1_ALU_Add_return_val;

proc_7_tSSA_8_4cc42779139d4e269b03bb7d3686bb3c : process(tSSA_8_4cc42779139d4e269b03bb7d3686bb3c)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
c <= tSSA_8_4cc42779139d4e269b03bb7d3686bb3c;
tSSA_9_4cc42779139d4e269b03bb7d3686bb3c <= a;
tSSA_10_4cc42779139d4e269b03bb7d3686bb3c <= b;
end if ResetSync;


end process proc_7_tSSA_8_4cc42779139d4e269b03bb7d3686bb3c;

proc_8_call_1_ALU_Sub_return_val : process(call_1_ALU_Sub_return_val)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
tSSA_11_4cc42779139d4e269b03bb7d3686bb3c <= call_1_ALU_Sub_return_val;
end if ResetSync;


end process proc_8_call_1_ALU_Sub_return_val;

proc_9_tSSA_11_4cc42779139d4e269b03bb7d3686bb3c : process(tSSA_11_4cc42779139d4e269b03bb7d3686bb3c)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
d <= tSSA_11_4cc42779139d4e269b03bb7d3686bb3c;
tSSA_12_4cc42779139d4e269b03bb7d3686bb3c <= a;
tSSA_13_4cc42779139d4e269b03bb7d3686bb3c <= b;
end if ResetSync;


end process proc_9_tSSA_11_4cc42779139d4e269b03bb7d3686bb3c;

proc_10_call_1_ALU_Mul_return_val : process(call_1_ALU_Mul_return_val)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
tSSA_14_4cc42779139d4e269b03bb7d3686bb3c <= call_1_ALU_Mul_return_val;
end if ResetSync;


end process proc_10_call_1_ALU_Mul_return_val;

proc_11_tSSA_14_4cc42779139d4e269b03bb7d3686bb3c : process(tSSA_14_4cc42779139d4e269b03bb7d3686bb3c)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
e <= tSSA_14_4cc42779139d4e269b03bb7d3686bb3c;
tSSA_15_4cc42779139d4e269b03bb7d3686bb3c <= a;
tSSA_16_4cc42779139d4e269b03bb7d3686bb3c <= b;
end if ResetSync;


end process proc_11_tSSA_14_4cc42779139d4e269b03bb7d3686bb3c;

proc_12_call_1_ALU_Div_return_val : process(call_1_ALU_Div_return_val)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
tSSA_17_4cc42779139d4e269b03bb7d3686bb3c <= call_1_ALU_Div_return_val;
end if ResetSync;


end process proc_12_call_1_ALU_Div_return_val;

proc_13_tSSA_17_4cc42779139d4e269b03bb7d3686bb3c : process(tSSA_17_4cc42779139d4e269b03bb7d3686bb3c)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
f <= tSSA_17_4cc42779139d4e269b03bb7d3686bb3c;
end if ResetSync;


end process proc_13_tSSA_17_4cc42779139d4e269b03bb7d3686bb3c;


end Behavioral;
