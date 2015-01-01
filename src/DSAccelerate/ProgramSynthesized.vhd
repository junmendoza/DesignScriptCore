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
signal call_1_Assign_return_val : std_logic_vector(31 downto 0);
signal x : std_logic_vector(31 downto 0);
signal tSSA_1_969ac5ccb9494486bb8d04eb98b96c6e : std_logic_vector(31 downto 0);
signal tSSA_2_969ac5ccb9494486bb8d04eb98b96c6e : std_logic_vector(31 downto 0);
signal y : std_logic_vector(31 downto 0);

component Assign is
port( 
	reset : in std_logic;
	a : in std_logic_vector(31 downto 0);
	return_Assign : out std_logic_vector(31 downto 0)
);
end component Assign;

begin
call_1_Assign : Assign port map
(
reset => reset,
a => tSSA_1_969ac5ccb9494486bb8d04eb98b96c6e,
return_Assign => call_1_Assign_return_val
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
tSSA_1_969ac5ccb9494486bb8d04eb98b96c6e <= x;
end if ResetSync;


end process proc_2_x;

proc_3_call_1_Assign_return_val : process(call_1_Assign_return_val)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
tSSA_2_969ac5ccb9494486bb8d04eb98b96c6e <= call_1_Assign_return_val;
end if ResetSync;


end process proc_3_call_1_Assign_return_val;

proc_4_tSSA_2_969ac5ccb9494486bb8d04eb98b96c6e : process(tSSA_2_969ac5ccb9494486bb8d04eb98b96c6e)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
y <= tSSA_2_969ac5ccb9494486bb8d04eb98b96c6e;
end if ResetSync;


end process proc_4_tSSA_2_969ac5ccb9494486bb8d04eb98b96c6e;


end Behavioral;
