library IEEE;

use IEEE.STD_LOGIC_1164.ALL;
use IEEE.NUMERIC_STD.ALL;

entity Assign is
port( 
	reset : in std_logic;
	a : in std_logic_vector(31 downto 0);
	return_Assign : out std_logic_vector(31 downto 0)
);
end Assign;

architecture Behavioral of Assign is



begin

proc_1_Assign : process(a)

begin
ResetSync : if reset = '1' then

elsif reset = '0' then
p <= X"00000064";
tSSA_0_3111717f2951425799ff62e670b79133 <= p;
return_Assign <= tSSA_0_3111717f2951425799ff62e670b79133;
end if ResetSync;


end process proc_1_Assign;


end Behavioral;
