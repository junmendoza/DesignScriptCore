----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    12:40:20 12/06/2014 
-- Design Name: 
-- Module Name:    SimpleExpression - Behavioral 
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
use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
--library UNISIM;
--use UNISIM.VComponents.all;


---------------------------
-- SimpleExpressionALU
-- 	a : int = 1
-- 	b : int = a + 2
--
--		allocate(a)
--		allocate(b)
--		a = 1
--		b = a + 2
---------------------------
entity SimpleExpression is
	Port( 
			clock : in  STD_LOGIC;
			reset : in  STD_LOGIC;
			
			-- in arg list
		
			-- out return list
			a_out : out STD_LOGIC_VECTOR(31 downto 0);
			b_out : out STD_LOGIC_VECTOR(31 downto 0)
	  );
end SimpleExpression;

architecture Behavioral of SimpleExpression is

	signal a : STD_LOGIC_VECTOR(31 downto 0);
	signal b : STD_LOGIC_VECTOR(31 downto 0);
	signal t0 : STD_LOGIC_VECTOR(31 downto 0);
	signal t1 : STD_LOGIC_VECTOR(31 downto 0);
	
	-- Flag to execute once
	signal exec : STD_LOGIC;

begin

	ProcExecute : process(clock)
	
	-- ALU variables
	variable iOp1 : integer;
	variable iOp2 : integer;
	variable tempDest : integer;
	
	begin 
		ResetSync : if reset = '1' then
			exec <= '1';
		elsif reset = '0' then
			ClockSync : if rising_edge(clock) then
				if exec = '1' then
					exec <= '0';
							
					a <= X"00000001";
					t0 <= X"00000002";
					
					iOp1 := to_integer(signed(a));
					iOp2 := to_integer(signed(t0));
					tempDest := iOp1 + iOp2;
					t1 <= std_logic_vector(to_signed(tempDest, 32));

					b <= t1;
					
					a_out <= a;
					b_out <= b;
				end if;
			end if ClockSync;
		end if ResetSync;
	end process ProcExecute;

end Behavioral;