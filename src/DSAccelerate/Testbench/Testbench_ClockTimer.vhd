--------------------------------------------------------------------------------
-- Company: 
-- Engineer:
--
-- Create Date:   23:00:14 02/07/2015
-- Design Name:   
-- Module Name:   D:/jun/Research/git/DesignScriptCore/src/DSAccelerate/Testbench/Testbench_ClockTimer.vhd
-- Project Name:  SynthesizedProgram
-- Target Device:  
-- Tool versions:  
-- Description:   
-- 
-- VHDL Test Bench Created by ISE for module: ClockTimer
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
 
ENTITY Testbench_ClockTimer IS
END Testbench_ClockTimer;
 
ARCHITECTURE behavior OF Testbench_ClockTimer IS 
 
    -- Component Declaration for the Unit Under Test (UUT)
 
    COMPONENT ClockTimer
    PORT(
			clock : in STD_LOGIC;
			reset : in STD_LOGIC;
			start : in STD_LOGIC;
			clockticks_elapsed : out STD_LOGIC_VECTOR(63 downto 0);
			ms_elapsed : out STD_LOGIC_VECTOR(31 downto 0)
        );
    END COMPONENT;
    

   --Inputs
   signal clock : std_logic := '0';
   signal reset : std_logic := '1';
   signal start : std_logic := '0';

 	--Outputs
   signal ms_elapsed : std_logic_vector(31 downto 0);
   signal clockticks_elapsed : std_logic_vector(63 downto 0);
 
BEGIN
 
	-- Instantiate the Unit Under Test (UUT)
   uut: ClockTimer PORT MAP (
          clock => clock,
          reset => reset,
          start => start,
          clockticks_elapsed => clockticks_elapsed,
          ms_elapsed => ms_elapsed
        );
		  

   -- Stimulus process
   stim_proc: process
   begin		
	
		reset <= '1';
		start <= '0';
		wait for 5 ns;		
		
		-- Begin execution
		reset <= '0';
		start <= '1';
		wait for 5 ns;
		
		for a in 1 to 1000000 loop
			clock <= not clock;
			wait for 1 ns;
			
			if a = 999995 then
				start <= '0';
			end if;
		end loop;
		
		wait;
   end process;

END;
