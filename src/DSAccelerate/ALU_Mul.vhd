----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    13:49:29 01/02/2015 
-- Design Name: 
-- Module Name:    ALU_Mul - Behavioral 
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

use IEEE.NUMERIC_STD.ALL;

library UNISIM;
use UNISIM.VComponents.all;

entity ALU_Mul is
    Port ( reset : in STD_LOGIC;
           op1 : in STD_LOGIC_VECTOR (31 downto 0);
           op2 : in STD_LOGIC_VECTOR (31 downto 0);
           result : out STD_LOGIC_VECTOR (31 downto 0));
end ALU_Mul;

architecture Behavioral of ALU_Mul is

begin

	process(op1, op2)
		
		variable iOp1 : integer;
		variable iOp2 : integer;
		variable tempDest : integer;
	
	begin
		if reset = '1' then
			result <= X"00000000";
		elsif reset = '0' then
			iOp1 := to_integer(signed(op1));
			iOp2 := to_integer(signed(op2));
			tempDest := iOp1 * iOp2;
			result <= std_logic_vector(to_signed(tempDest, 32));
		end if;
	end process;
end Behavioral;

