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

signal a : std_logic_vector(31 downto 0);
signal b : std_logic_vector(31 downto 0);


begin

proc_1_ProgramSynthesized : process(clock)

begin
ResetSync : if reset = '1' then
execution_started <= '0';

elsif reset = '1' then
ClockSync : if rising_edge(clock) then
execution_started <= '1';

end if ClockSync;
end if ResetSync;


end process proc_1_ProgramSynthesized;


end Behavioral;
