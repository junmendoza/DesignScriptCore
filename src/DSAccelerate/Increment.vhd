----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    22:15:15 12/29/2014 
-- Design Name: 
-- Module Name:    Increment - Behavioral 
-- Project Name: 
-- Target Devices: 
-- Tool versions: 
-- Description: 
--
-- Dependencies: 
--
-- Revision: 
-- Revision 0.01 - File Created
-- Additional Comments: 
--
----------------------------------------------------------------------------------
library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

-- def Increment(a : int)
-- {
--		t = a + i;
--		return = t;
-- }
entity Increment is
	Port( 
			reset : in STD_LOGIC;
			a : in STD_LOGIC_VECTOR(31 downto 0);
			return_val : out STD_LOGIC_VECTOR(31 downto 0)
		 );
end Increment;

architecture Behavioral of Increment is

	signal ALU1_Add_return : STD_LOGIC_VECTOR(31 downto 0);
	
	component ALU_Add is
		Port( 
				reset : in STD_LOGIC;
				op1 : in STD_LOGIC_VECTOR (31 downto 0);
				op2 : in STD_LOGIC_VECTOR (31 downto 0);
				result : out  STD_LOGIC_VECTOR (31 downto 0)
			  );
	end component ALU_Add;
	
begin

	ALU1_Add : ALU_Add port map
	(
		reset => reset,
		op1 => a,
		op2 => X"00000001",
		result => ALU1_Add_return
	);

	-- entry process
	proc1_return_ALU_Add : process(ALU1_Add_return)
	begin
		ResetSync : if reset = '1' then
		
		elsif reset = '0' then
			return_val <= ALU1_Add_return;
					
		end if ResetSync;
	end process proc1_return_ALU_Add;

end Behavioral;

