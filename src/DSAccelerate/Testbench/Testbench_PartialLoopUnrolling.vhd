--------------------------------------------------------------------------------
-- Company: 
-- Engineer:
--
-- Create Date:   17:58:29 12/24/2014
-- Design Name:   
-- Module Name:   D:/jun/Research/git/DSAccelerate/Testbench/Testbench_PartialLoopUnrolling.vhd
-- Project Name:  SynthesizedProgram
-- Target Device:  
-- Tool versions:  
-- Description:   
-- 
-- VHDL Test Bench Created by ISE for module: PartialLoopUnrolling
-- 
-- Dependencies:
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
--
-- Notes: 
-- This testbench has been automatically generated using types std_logic and
-- std_logic_vector for the ports of the unit under test.  Xilinx recommends
-- that these types always be used for the top-level I/O of a design in order
-- to guarantee that the testbench will bind correctly to the post-implementation 
-- simulation model.
--------------------------------------------------------------------------------
LIBRARY ieee;
USE ieee.std_logic_1164.ALL;
 
-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--USE ieee.numeric_std.ALL;
 
ENTITY Testbench_PartialLoopUnrolling IS
END Testbench_PartialLoopUnrolling;
 
ARCHITECTURE behavior OF Testbench_PartialLoopUnrolling IS 
 
    -- Component Declaration for the Unit Under Test (UUT)
 
    COMPONENT PartialLoopUnrolling
    PORT(
         clock : IN  std_logic;
         reset : IN  std_logic
        );
    END COMPONENT;
    

   --Inputs
   signal clock : std_logic := '0';
   signal reset : std_logic := '1';
 
BEGIN
 
	-- Instantiate the Unit Under Test (UUT)
   uut: PartialLoopUnrolling PORT MAP (
          clock => clock,
          reset => reset
        );

   -- Stimulus process
   stim_proc: process
   begin		
	
		reset <= '1';
		wait for 5 ns;		
		
		-- Begin execution
		reset <= '0';
		wait for 5 ns;
		
		for a in 1 to 10 loop
			clock <= not clock;
			wait for 5 ns;
		end loop;
		
      wait;
   end process;

END;
